using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class CustomerDemographicRepository : ICustomerDemographicRepository
{
    private readonly ApplicationDbContext applicationDbContext;

    public CustomerDemographicRepository(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public async Task<List<CustomerDemographic>> GetAllCustomerDemographicsAsync()
    {
        var customerDemograhics = await applicationDbContext.CustomerDemographics.ToListAsync();
        customerDemograhics.ForEach(x => x.CustomerTypeID = x.CustomerTypeID.Trim());
        return customerDemograhics;
    }

    public async Task<CustomerDemographic> GetCustomerDemographicByIdAsync(string? id)
    {
        var customerDemographic = await applicationDbContext.CustomerDemographics.FirstOrDefaultAsync(cd => cd.CustomerTypeID == id);
        if (customerDemographic != null)
        {
            customerDemographic.CustomerTypeID = customerDemographic.CustomerTypeID.Trim();
        }

        return customerDemographic;
    }

    public async Task<CustomerDemographic> AddCustomerDemographicAsync(CustomerDemographic customerDemographic)
    {
        if (customerDemographic == null)
        {
            throw new ArgumentNullException(nameof(customerDemographic));
        }

        await applicationDbContext.CustomerDemographics.AddAsync(customerDemographic);
        await applicationDbContext.SaveChangesAsync();

        return customerDemographic;
    }

    public async Task<CustomerDemographic> EditCustomerDemographicAsync(CustomerDemographic customerDemographic)
    {
        var customerDemographicToUpdate = await applicationDbContext.CustomerDemographics.FirstOrDefaultAsync(cd => cd.CustomerTypeID == customerDemographic.CustomerTypeID);

        if (customerDemographicToUpdate == null)
        {
            return null;
        }

        customerDemographicToUpdate.CustomerDesc = customerDemographic.CustomerDesc;

        applicationDbContext.CustomerDemographics.Update(customerDemographicToUpdate);
        await applicationDbContext.SaveChangesAsync();
        return customerDemographicToUpdate;
    }

    public async Task<bool> DeleteCustomerDemographicAsync(string? id)
    {
        var customerDemographic = await applicationDbContext.CustomerDemographics.FirstOrDefaultAsync(cd => cd.CustomerTypeID == id);

        if (customerDemographic == null)
        {
            return false;
        }

        applicationDbContext.CustomerDemographics.Remove(customerDemographic);
        await applicationDbContext.SaveChangesAsync();
        return true;
    }
}