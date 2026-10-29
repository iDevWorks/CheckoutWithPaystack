using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack;

public class CustomerClient(PaystackClient paystack)
{
    public async Task<Customer?> Get(string customerCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerCode);
        try
        {
            return await paystack.GetAsync<Customer>($"customer/{customerCode}", ct);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public Task<Customer> Create(string email, string firstName, string lastName, string phone, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(phone);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);

        var body = new
        {
            email = email,
            phone = phone,
            first_name = firstName,
            last_name = lastName,
        };

        return paystack.PostAsync<Customer>("customer", body, ct);
    }

    public Task<Customer> Update(string customerCode, string firstName, string lastName, string phone, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(phone);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);

        var body = new
        {
            phone = phone,
            first_name = firstName,
            last_name = lastName,
        };

        return paystack.PutAsync<Customer>($"customer/{customerCode}", body, ct); //PUT not POST
    }
}