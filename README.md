# WCF-Library #

## Description ##
Należy zrealizować usługę pozwalająca na:

- wyszukiwanie książki na podstawie słowa w tytule. Metoda usługi przyjmuje jako argument łańcuch tekstowy i zwraca listé identyfikatorów książkiek typu integer

- pobieranie szczegółów książki. Metoda przyjmuje jako argument identyfikator książki typu integer i zwraca strukturę zawierającą tytuł i listę autorów (lista struktur: imię, nazwisko). W przypadku niepoprawnego identyfikatora usługa zgłasza błąd (należy zdefiniować klasę dla kontrkatu błędów)

Usługa może mieć predefiniowaną listę kilku książek. Klient w fromie aplikacji konsolowej powinien wykonywać przykładowe operacje, wyświetlać szczegóły odnalezionych książek i poprawnie obsługiwać błędy.

## Specifications ##

### LibraryService (WCFService) ###

This project contains only the definitions of the service contract and data contracts, as we have already established.

1. Service Contract: Defines the operations that the service exposes. This is done using interfaces annotated with [ServiceContract] and methods within those interfaces annotated with [OperationContract].
2. Data Contracts: Defines the data structures used in the service. These are classes annotated with [DataContract] and their members with [DataMember].

### Server(Console App Framework) ###

This project hosts the WCF service and provide the implementation of the service functionality.

### Client(Console App Framework) ###

This project consumes the WCF service by creating a proxy to the service and calling its operations.
