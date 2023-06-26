### Bulk Importing new entries to IAMLegacyAuthOrgStore table

1. Run below query on SQL Server Management Studio to create the dataset to import. Replace the @clientId variable with new ClientId. Customize the query to select necessary organizations
```
declare @clientId varchar(120)='b06f9f23-50a7-400e-8bf2-83dbc75d1862'
SELECT DISTINCT 
	LOWER(OrganizationId) as PartitionKey,@clientId RowKey, 
	LOWER(OrganizationId) OrganizationId, 
	'Guid' as 'OrganizationId@type',
	@clientId  ClientId, 
	'String' as 'ClientId@type'
FROM dbo.OrganizationSettings 
WHERE PalaceSettingsEnabled=1
```

2. Export the Result to a CSV file

3. Add below headers to CSV files and Save. Make sure the CSV file has 'UTF8' encoding while saving.
``` 
PartitionKey,RowKey,OrganizationId,OrganizationId@type,ClientId,ClientId@type 
```

4. Open the target Azure Table from Azure Table Storage Explorer tool and Use the Import option to import the file

**NOTE**:  Importing new entries won't remove existing entries in Azure Table