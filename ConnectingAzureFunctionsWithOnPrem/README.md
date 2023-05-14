# Connecting Azure Function Apps or Web Apps with On-Prem SQL Server

In today's digital landscape, seamless integration between cloud-based ***REMOVED***lications and on-premises resources is often a requirement for businesses. This integration can involve scenarios such as connecting Azure Function Apps or Web Apps with on-premises SQL Server or migrating from one cloud provider to another, such as from AWS to Azure.

For the project I'm currently working on, we encountered the challenge of migrating a large legacy platform running on AWS. The platform consisted of a SQL Server database hosted within a VM and multiple .NET framework-based ***REMOVED***lications hosted on separate VMs within a VPC (Virtual Private Cloud/Virtual Network). Our goal was to gradually migrate the ***REMOVED***lication stack from AWS IaaS to Azure PaaS offering, leveraging Azure Functions, Azure App Services, DevOps, etc.

To begin the migration, we first focused on rewriting the critical backend components using the latest .NET framework and incorporating Azure Functions. However, a significant challenge arose due to the existing SQL Server database residing in AWS within a VPC. Our priority was to establish a secure connectivity solution that would allow Azure Functions to connect to the AWS SQL Server instance. This project necessitated careful planning and exploration of secure connectivity options between Azure and the AWS SQL Server database.

## Prerequisites

Before testing the options, ensure you have below 2 function ***REMOVED***s created and running 2 seperate instances with this project code base. You can create the azure environment by running below script using Azure CLI.

```
$resourceGroup = 'azure-relay-function***REMOVED***-test'
$resourceLocation = 'australiaeast'
$***REMOVED***ServicePlanName = 'zure-relay-function***REMOVED***-plan'
$storageAccountName = 'azrelayfunc***REMOVED***test'

az group create --name $resourceGroup --location $resourceLocation
az ***REMOVED***service plan create --name $***REMOVED***ServicePlanName --resource-group $resourceGroup --sku B1 --location $resourceLocation
az storage account create --name $storageAccountName --resource-group $resourceGroup --location $resourceLocation --sku Standard_LRS
az function***REMOVED*** create --name azure-relay-function***REMOVED***-test --resource-group $resourceGroup --plan $***REMOVED***ServicePlanName --runtime dotnet --os-type Windows --functions-version 4 --storage-account $storageAccountName
az function***REMOVED*** create --name azure-relay-function***REMOVED***-test2 --resource-group $resourceGroup --plan $***REMOVED***ServicePlanName --runtime dotnet --os-type Windows --functions-version 4 --storage-account $storageAccountName
```

## Solution #1 - Hybrid Connection Using Azure Relay
When establishing connectivity between an Azure PaaS service and SQL Server running on an VM within an On-prem or Azure virtual network, the traditional ***REMOVED***roach often involves routing all outbound traffic from the Azure PaaS service to a forwarding VNet. This forwarding VNet is then connected to the target VNet through VNet peering or, in the case of on-premises or another cloud VNet, VPN tunneling is used. However, this ***REMOVED***roach requires significant infrastructure network changes and may necessitate enabling new inbound firewall rules as well.

Without this complex infrastructure alteration, Azure provides alternate option on connecting these resources called Azure Relay Hybrid Connection. Instead PaaS service ***REMOVED*** to connect to SQL server running VM using inbound connections, in this ***REMOVED***roach, a **Relay Agent** will be deployed on the target SQL server running VM. This Relay Agent (aka Hybrid Connection Manager - HCM), calls out to Azure Relay service over port 443 (TLS) as a listner. Also, from App Service, App Service infrastructure also connects to Azure Relays on our ***REMOVED***lication behalf as a sender. Through this joined connect our ***REMOVED*** will be able to communicate with the target SQL server instance. When the App Service make outbound connections to a resource using DNS which registered for this Hybrid Connections, that traffic will be forward to Azure Relay. From their Azure Relay will forward this traffic to listners at the other end via Bi-Directional dedicated Web-socket connection or HTTP. Wich allows to send requests and receive responses.

![image](https://github.com/DHJayasinghe/system-design/assets/26274468/338149dd-a994-42e2-8c16-dd7eb1c69d8a)
(Reference: https://learn.microsoft.com/en-us/azure/***REMOVED***-service/***REMOVED***-service-hybrid-connections#how-it-works)

### Steps for Setup
1. Go to Azure Function Apps/Web Apps You created on Prerequisite section. Navigate to Networking -> Outbound Traffic -> Hybrid Connections.
2. Add new Hybrid Connection named 'SqlConnection'. Make sure you are using Named instance of SQL server. Because traffic forwarding from App Service to Azure Relay service is based on DNS. So, pointing IP won't work here. For the Endpoint Host pass the "SQL SERVER INSTANCE NAME". And for Service Bus namespace provide a unique namespace for your connection.
![image](https://github.com/DHJayasinghe/system-design/assets/26274468/e26065b6-3250-4e5b-9472-82151a668ef2)
3. After that click the 'Download Configuration Manager' button which gonna download an executable file.
![image](https://github.com/DHJayasinghe/system-design/assets/26274468/c7501d92-bcda-4121-888c-03857530328e)
4. Copy that executable to the target VM where your SQL server instance is running inside VM. Install it. After installation search for "Hybrid Connection Manager UI" and run the ***REMOVED***lication.
5. Once the ***REMOVED***lication is open. Add New Hybrid Connection Using Enter manually option. Else you can sign-in to portal from that UI and pick it up the Relay Service from there too. But it will require re-authentication inside that VM. So go for the enter manual option.
6. For that you need the connection string of the Relay service. Which can found under the Resource Group where you created Azure Function Hybrid Connection. Go inside that Relay resource. 
![image](https://github.com/DHJayasinghe/system-design/assets/26274468/13bb81ab-a0cb-4fde-a13d-9cd372fa2055)
And go to **Hybrid Connections** -> Select the Connection You Created -> Shared access policies -> Pick the **defaultListener** Connection String and Copy it to Manual En***REMOVED*** option on Hybrid Connection Manager UI.
![image](https://github.com/DHJayasinghe/system-design/assets/26274468/00615fa3-ed4b-4bcb-ad1e-bdfa1b55cb5f)
7. Once that done restart the **Azure Hybrid Connection Manager Service** from the Services in the VM.
![image](https://github.com/DHJayasinghe/system-design/assets/26274468/dfc1a4c8-cd46-4820-93c1-5f0bab2c1497)
8. Once that done, you will see Connected status on **Hybrid Connection Manager UI**. Then your ***REMOVED***lication should be able to connect to the SQL server running on VM. 
![image](https://github.com/DHJayasinghe/system-design/assets/26274468/aeb74429-cda1-42f6-babb-70de709a7490)
9. You can verify this by going to Function Apps. Console section and run the below command. Here 'Dhanuka' is my SQL Server named instance on my Local machine. I'm connecting to my local SQL server using Azure Relay Hybrid Connection from the Function App running in the Azure. If the connection successfull you should see below response.
![image](https://github.com/DHJayasinghe/system-design/assets/26274468/307200f7-2558-4f75-a105-4842e62fa404)

 





