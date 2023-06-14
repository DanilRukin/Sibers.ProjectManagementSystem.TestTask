# Sibers.ProjectManagementSystem
## Описание
ProjectManagementSystem - система управления проектами, разработанная, как тестовое задание для [Sibers](https://sibers.ru/).

Программное обеспечение предоставляет следующие возможности:

* Возможность создавать/просматривать/редактировать/удалять информацию о проектах
* Возможность создавать/просматривать/редактировать/удалять информацию о сотрудниках
* Возможность добавлять и удалять сотрудников c проекта
* Для просмотра проектов предусмотрены различные способы фильтрации



## Технологии
В данном проекте были использованы следующие технологии:
- [MudBlazor](https://mudblazor.com/)
- [MediatR](https://github.com/jbogard/MediatR)
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- [Entity Framework Core](https://learn.microsoft.com/ru-ru/ef/core/) (приложение может работать со следующими хранилищами):
    - SQL Server
    - PostgreSQL
    - SQLite
    - InMemoryDatabase (только для тестирования)
- [xUnit](https://xunit.net/)
- [Moq](https://github.com/moq/moq)
- [Swagger](https://swagger.io/)



## Установка и запуск
Для успешного запуска приложения необходимо проделать несколько вещей, а именно:
- Опубликовать проект [Sibers.ProjectManagementSystem.Api](src/Sibers.ProjectManagementSystem.Api/)
- Опубликовать проект [Sibers.ProjectManagementSystem.Presentation.Blazor](src/Sibers.ProjectManagementSystem.Presentation.Blazor/)
- Если хотите использовать постоянное хранилище, то необходимо настроить для использования одну из СУБД
    - SQL Server
    - PostgreSQL
- Настроить для использования один из файловых серверов. В данном случае [IIS](https://www.iis.net/)

### Установка и настройка IIS
Для установки и настройки IIS используйте [это руководство](https://learn.microsoft.com/ru-ru/iis/manage/creating-websites/scenario-build-a-static-website-on-iis).

### Настройка СУБД (PostgreSQL)
Для работы с PostgreSQL установите и настройте кластер, используя стандартные средства. Создавать базу данных на сервере **не нужно**, она будет создана автоматически. Затем, в опубликованном проекте [Sibers.ProjectManagementSystem.Api](src/Sibers.ProjectManagementSystem.Api/) в файле **appsettings.json** выполните необходимые настройки ([далее](README.md#развертывание-и-конфигурация-api)).

### Развертывание и конфигурация API
Используя [Visual Studio](https://visualstudio.microsoft.com/ru/downloads/) либо команду `dotnet publish`, опубликуйте проект [Sibers.ProjectManagementSystem.Api](src/Sibers.ProjectManagementSystem.Api/) в папку. В опубликованных файлах найдите файл **appsettings.json**. Этот файл содержит несколько секций:
* Logging - отвечает за настройку логгирования
* AllowedHosts - указывает, с каких адресов можно обращаться к серверу
* Profiles - содержит профили хранилищ для использования
	* SqlServerProfile - профиль для настройки доступа к SQL Server
	* PostgresProfile - профиль для настройки доступа к PostgreSQL
	* SQLiteProfile - профиль для настройки доступа к SQLite (в данном случае в памяти) **Не рекомендуется использовать SQLite в памяти**
	* InMemoryProfile - поднимется база данных в памяти
* UseProfile - указывает, какой профиль использовать

#### Profiles

Каждый профиль содержит минимум 4 свойства:
* ConnectionString - строка подключения для БД
* UseSeedData - флаг использования тестовых данных
* MigrateDatabase - если UseSeedData установлен в true, то указывает применять ли миграции
* CreateDatabase - если UseSeedData установлен в true, то указывает создавать ли БД 'внаглую'

Вам необходимо в **UseProfile** указать любой из предоставленных профилей (SqlServerProfile, SQLiteProfile, InMemoryProfile). Т.к. необходима работа с SQL Server, то в UseProfile указываем SqlServerProfile.



#### Рекомендации по настройке профиля SqlServerProfile

* ConnectionString - здесь поменяйте адрес сервера с базой данных (схемой), имя пользователя, пароль и порт при необходимости
* UseSeedData - установите в true, чтобы была заполнена тестовая база данных **ВНИМАНИЕ: База данных с именем, указанным в _ConnectionString_ дложна быть создана _пустой_ на сервере БД до старта API (не касается PostgreSQL)**
* MigrateDatabase - установите в true
* CreateDatabase - установите в false
* MigrationAssembly - не трогайте. Это путь к сборке, в которой содержится сгенерированный Ef Core код миграций.

Если захотите поменять хранилище, то просто установите **UseProfile** с нужным именем профиля **(SQLiteProfile - НЕ НАДО)**

#### Публикация на IIS
После всех изменений, скопируйте опубликованные файлы на сервер IIS (как правило, это одна из директорий внутри C:\inetpub\ ). Создайте сайт на IIS с помощью мастера создания сайтов, используя [Диспетчер служб IIS](https://learn.microsoft.com/en-us/iis/get-started/getting-started-with-iis/getting-started-with-the-iis-manager-in-iis-7-and-iis-8). **Запомните порт, на котором вы опубликовали API**.


### Развертывание и конфигурация веб-клиента
Используя [Visual Studio](https://visualstudio.microsoft.com/ru/downloads/) либо команду `dotnet publish`, опубликуйте проект [Sibers.ProjectManagementSystem.Presentation.Blazor](src/Sibers.ProjectManagementSystem.Presentation.Blazor/) в папку. В опубликованных файлах найдите файл **appsettings.json**. В этом файле измените значение секции **ApiBaseAddress** таким образом, чтобы это был адрес вашего API, который вы развернули на предыдущем шаге.


После всех изменений, скопируйте опубликованные файлы на сервер IIS (как правило, это одна из директорий внутри C:\inetpub\ ). Создайте сайт на IIS с помощью мастера создания сайтов, используя [Диспетчер служб IIS](https://learn.microsoft.com/en-us/iis/get-started/getting-started-with-iis/getting-started-with-the-iis-manager-in-iis-7-and-iis-8).

***Поздравляем! Ваше приложение работает!***


### Запуск из Visual Studio

- Склонируйте репозиторий к себе на локальный компьютер.
- В свойствах решение установите несколько запускаемых проектов.
- В файле **appsettings.json** проекта [Sibers.ProjectManagementSystem.Api](src/Sibers.ProjectManagementSystem.Api/) установите значение секции **UseProfile** равным **InMemoryProfile**
- Запустите приложение

***Поздравляем! Ваше приложение работает!***
