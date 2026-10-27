using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack.VirtualAccount;

public class Client(PaystackClient paystack)
{
    public const string DefaultBank = "titan-paystack"; //wema, titan-paystack, test-bank

    public async Task<DedicatedAccount> GetOrCreateVirtualAccount(
        string email, string firstName, string lastName, string phone, 
        string preferredBank = DefaultBank, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(phone);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(preferredBank);

        // 1) Existing customer + existing DVA?
        var customer = await paystack.Customer.GetCustomerAsync(email, ct);

        if (customer is not null && customer.DedicatedAccount is not null /*&& customer.DedicatedAccount.Active*/)
            return customer.DedicatedAccount;

        // 2) Make sure the customer exists and has the fields Paystack needs for a DVA.
        customer = customer is null
            ? await paystack.Customer.CreateCustomerAsync(email, firstName, lastName, phone, ct)
            : await paystack.Customer.UpdateCustomerAsync(customer.CustomerCode, firstName, lastName, phone, ct);

        // 3) Create the DVA.
        return await CreateVirtualAccount(customer.CustomerCode, firstName, lastName, phone, preferredBank, ct);
    }

    public Task<DedicatedAccount> CreateVirtualAccount(string customerCode, 
        string? firstName, string? lastName, string? phone, 
        string preferredBank = DefaultBank, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerCode);

        var body = new
        {
            phone = phone,
            last_name = lastName,
            first_name = firstName,
            customer = customerCode,
            preferred_bank = preferredBank,
        };

        return paystack.PostAsync<DedicatedAccount>("dedicated_account", body, ct);
    }

    public Task AssignVirtualAccount(string email, string firstName, string lastName,
        string phone, string preferredBank = DefaultBank, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(phone);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(preferredBank);

        var body = new
        {
            email = email,
            phone = phone,
            last_name = lastName,
            first_name = firstName,
            preferred_bank = preferredBank,
            country = "NG",
        };

        return paystack.PostAsync<DedicatedAccount>("dedicated_account/assign", body, ct);
    }

    public Task<List<DedicatedAccount>> ListVirtualAccounts(string? customerCode, bool? active, string currency = "NGN", CancellationToken ct = default)
    {
        var url = $"dedicated_account?currency={currency}";

        if (customerCode != null)
            url += $"&customer={customerCode}";

        if (active != null)
            url += $"&active={active}";

        return paystack.GetAsync<List<DedicatedAccount>>(url, ct);
    }

    public Task<DedicatedAccount> FetchVirtualAccount(string virtualAccountCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(virtualAccountCode);

        return paystack.GetAsync<DedicatedAccount>(
            $"dedicated_account/{virtualAccountCode}", ct);
    }

    public Task<DedicatedAccount> DeleteVirtualAccount(string virtualAccountCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(virtualAccountCode);

        return paystack.DeleteAsync<DedicatedAccount>(
            $"dedicated_account/{virtualAccountCode}", ct);
    }

    public async Task RequeryVirtualAccount(string accountNumber, string providerCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(providerCode);

        await paystack.GetAsync<string>(
            $"dedicated_account/requery?account_number={accountNumber}&provider_slug={providerCode}", ct);
    }

    public Task<List<BankProviderResponse>> ListProviderBanks(CancellationToken ct = default)
    {
        return paystack.GetAsync<List<BankProviderResponse>>(
            "dedicated_account/available_providers", ct);
    }
}