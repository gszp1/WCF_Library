# WCF-Library #

## Used technologies ##

1. .NET Framework 4.8
2. Windows Communication Foundation

## Description ##

The goal of this project is to implement a library service that will allow connected clients to:

1. Get identifiers of books that have provided keyword in title.
2. Get details of book with provided identifier.

## Specifications ##

- Connection configuration for both client and server are stored in App.Config files.
- Connection between client and server is established via TCP.
- Project consists of three sub-projects: WCFService Library and two Console projects.

### LibraryService (WCFService) ###

This project contains only the definitions of the service contract and data contracts, as we have already established.

1. Service Contract: Defines the operations that the service exposes. This is done using interfaces annotated with [ServiceContract] and methods within those interfaces annotated with [OperationContract].
2. Data Contracts: Defines the data structures used in the service. These are classes annotated with [DataContract] and their members with [DataMember].

### Server(Console App Framework) ###

This project hosts the WCF service and provide the implementation of the service functionality.

### Client(Console App Framework) ###

This project consumes the WCF service by creating a proxy to the service and calling its operations.
