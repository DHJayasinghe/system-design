# Connecting Azure Function Apps or Web Apps with On-Prem SQL Server

In today's digital landscape, seamless integration between cloud-based applications and on-premises resources is often a requirement for businesses. This integration can involve scenarios such as connecting Azure Function Apps or Web Apps with on-premises SQL Server or migrating from one cloud provider to another, such as from AWS to Azure.

For the project I'm currently working on, we encountered the challenge of migrating a large legacy platform running on AWS. The platform consisted of a SQL Server database hosted within a VM and multiple .NET framework-based applications hosted on separate VMs within a VPC (Virtual Private Cloud/Virtual Network). Our goal was to gradually migrate the application stack from AWS IaaS to Azure PaaS offering, leveraging Azure Functions, Azure App Services, DevOps, etc.

To begin the migration, we first focused on rewriting the critical backend components using the latest .NET framework and incorporating Azure Functions. However, a significant challenge arose due to the existing SQL Server database residing in AWS within a VPC. Our priority was to establish a secure connectivity solution that would allow Azure Functions to connect to the AWS SQL Server instance. This project necessitated careful planning and exploration of secure connectivity options between Azure and the AWS SQL Server database.

## Prerequisites

Before testing the options, ensure you have below 2 function apps created and running 2 seperate instances with this project code base. You can create the azure environment by running below script using Azure CLI.

```
$resourceGroup = 'azure-relay-functionapp-test'
$resourceLocation = 'australiaeast'
$appServicePlanName = 'zure-relay-functionapp-plan'
$storageAccountName = 'azrelayfuncapptest'

az group create --name $resourceGroup --location $resourceLocation
az appservice plan create --name $appServicePlanName --resource-group $resourceGroup --sku B1 --location $resourceLocation
az storage account create --name $storageAccountName --resource-group $resourceGroup --location $resourceLocation --sku Standard_LRS
az functionapp create --name azure-relay-functionapp-test --resource-group $resourceGroup --plan $appServicePlanName --runtime dotnet --os-type Windows --functions-version 4 --storage-account $storageAccountName
az functionapp create --name azure-relay-functionapp-test2 --resource-group $resourceGroup --plan $appServicePlanName --runtime dotnet --os-type Windows --functions-version 4 --storage-account $storageAccountName
```

## Solution #1 - Hybrid Connection Using Azure Relay
When establishing connectivity between an Azure PaaS service and SQL Server running on an VM within an On-prem or Azure virtual network, the traditional approach often involves routing all outbound traffic from the Azure PaaS service to a forwarding VNet. This forwarding VNet is then connected to the target VNet through VNet peering or, in the case of on-premises or another cloud VNet, VPN tunneling is used. However, this approach requires significant infrastructure network changes and may necessitate enabling new inbound firewall rules as well.

Without this complex infrastructure alteration, Azure provides alternate option on connecting these resources called Azure Relay Hybrid Connection. Instead PaaS service try to connect to SQL server running VM using inbound connections, in this approach, a **Relay Agent** will be deployed on the target SQL server running VM. This Relay Agent (aka Hybrid Connection Manager - HCM), calls out to Azure Relay service over port 443 (TLS) as a listner. Also, from App Service, App Service infrastructure also connects to Azure Relays on our application behalf as a sender. Through this joined connect our app will be able to communicate with the target SQL server instance. When the App Service make outbound connections to a resource using DNS which registered for this Hybrid Connections, that traffic will be forward to Azure Relay. From their Azure Relay will forward this traffic to listners at the other end via Bi-Directional dedicated Web-socket connection or HTTP. Wich allows to send requests and receive responses.


