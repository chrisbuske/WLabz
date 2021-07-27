# WLabz
 WonderLabz

 - Requires running instance of RabbitMQ for transaction auditing exchange
 
 - Database and RabbitMQ configurations stored in the appsettings.json configuratin file
 
 - Run the wlabz.api.clientelle application first and execute admin api as presented to create data storage entities in the data store
 
 - The solution is divided into 5 projects:

 Project 1: Wlabz.Logs
 Event Infrastructure Interaction Audit Logging utilizing RabbitMQ. Further development would require the pulling of the messages from the relevant Rabbit MQ exchange for further processing
 
 Project 2: WLabz.Repository
 Infrastructure Interaction for Data Storage utilizing NHibernate
 
 Project 3: wlabz.api.clientelle
 Manages the Bank clients 
 (I have included all data entities in this project simply to simplify the creation of the data storage environment for testing purposes. This would not be my approach in production code as it creates reliance between projects)
 
 Project 4: wlabz.api.accounts.savings &  Project 5: wlabz.api.accounts.current
I separated savings and current accounts into two projects as within banks there are often different implementation teams. This also enables separation of concern and enables non interdependence of teams.
 

 
 
