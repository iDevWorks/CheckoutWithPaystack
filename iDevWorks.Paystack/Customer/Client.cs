using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack.Customer;

public class Client(PaystackClient paystack)
{
    public async Task<CustomerResponse?> GetCustomerAsync(string customerCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerCode);
        try
        {
            return await paystack.GetAsync<CustomerResponse>($"customer/{customerCode}", ct);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public Task<CustomerResponse> CreateCustomerAsync(string email, string firstName, string lastName, string phone, CancellationToken ct = default)
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

        return paystack.PostAsync<CustomerResponse>("customer", body, ct);
    }

    public Task<CustomerResponse> UpdateCustomerAsync(string customerCode, string firstName, string lastName, string phone, CancellationToken ct = default)
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

        return paystack.PutAsync<CustomerResponse>($"customer/{customerCode}", body, ct); //PUT not POST
    }
}