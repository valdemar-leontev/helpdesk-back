# Helpdesk readme

## Q1) How to create the first code migration EFCore in ASP.NET Core?

1. Add package to Microsoft.EntityFrameworkCore.Design to project which contains database context.

        cd ./Helpdesk.DataAccess
        dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.6

2. Implement a constructor without parameters of the DbContext inheritance

        public class AppDatabaseContext : DbContext
        {
          public AppDatabaseContext()
          {
          }
          ...
        }
3. Run a migration process
        cd ./Helpdesk.WebApi
        dotnet ef migrations add InitialCreate --output-dir DbMigrations

4. Make sure that database context will be added to DI container of ASP.NET Core and Migrate method will be executed

        public void ConfigureServices(IServiceCollection services)
        {
          services.AddDbContext<AppDatabaseContext>(options =>
          {
              var cs = Configuration.GetConnectionString("DefaultConnection");
              options.UseNpgsql(cs, options => {
                  options.MigrationsAssembly("Helpdesk.WebApi");
              });
          });

          ...

        }

          public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDatabaseContext dataContext)
        {
            dataContext.Database.Migrate();

            ....

          }

5. Run app and make sure than database was created and if everything in order then be happy!

## Q2) What about EF Core?

1. Creating model and construction relations

<https://docs.microsoft.com/en-us/ef/core/modeling/>

2. Code first migrations strategy

<https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli>

3. Npgsql Entity Framework Core Provider (Important to read!)

<https://www.npgsql.org/efcore/index.html>

4. Tutorial and a recompilation of an official documentation
<https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx>

## Q3) And what if you have no an experience with SSH

1. PuTTY
To connect with a remote server for example Ubuntu you have to have a private key file (*.ppk) and add it into Pageant (PuTTY Auth Key Agent). After that you have to create a session with a properly appointed IP, port and username. At last connect to created session and use BASH.
But you will not have an enjoy at all!

3. WinSCP
That is a very good app, it works together PuTTY and allows to transfer file from your host to a remote server.

4. Pay attention to it! VSCode provides the most convenient abilities for communicate over SSH. Install a special extensions "Remote - SSH", "Remote - Containers" and "Remote - SSH: Editing Configuration Files". After that select in the Remote Explorer "SSH Targets" and add item with correct properties. Then edit ssh config on your local machine (C:\Users\Leo\.ssh\config), for example:

Host 89.22.181.147
  HostName 89.22.181.147
  User ext_test
  Port 2222
  IdentityFile ~/.ssh/ext_test

VSCode SSH plugins require OpenSSH format for private keys, that's why you need to convert PuTTY (*.ppk) key to OpenSSH, for example through PuTTYgen and place new keys to C:\Users\Leo\.ssh\ folder.

## Q4) And what to do if I am an absolutely newly in the Linux?

1. At first learn several important commands of a command line processor of Linux
<https://maker.pro/linux/tutorial/basic-linux-commands-for-beginners>

2. The most simplest way using Docker Engine for deploy your developed application on the Linux
<https://docs.docker.com/engine/install/ubuntu/>

3. Use WSL2 on the Windows to develop and deploy cross platform applications. VSCode supplies a complete set of tools for working with SSH and WSL/WSL2
<https://code.visualstudio.com/blogs/2019/09/03/wsl2>
<https://code.visualstudio.com/blogs/2020/07/01/containers-wsl>

4. Learn shell scripting
<https://www.tutorialspoint.com/unix/shell_scripting.htm>

## Q5) How to apply changes from .gitignore

1. git rm -r --cached .

2. git add .

3. git commit -m 'your commit message'

4. git push

## Q6) What about multi-scheme authentication in ASP.NET Core?

1. See that post:

<https://stackoverflow.com/questions/45695382/how-do-i-setup-multiple-auth-schemes-in-asp-net-core-2-0>

## Q7) How to deploy APS.NET Core app to  on Linux Server (without docker / docker engine)?

<https://www.c-sharpcorner.com/article/how-to-deploy-net-core-application-on-linux/>

## Q8) LDAP
<https://stackoverflow.com/questions/11580128/how-to-convert-sid-to-string-in-net>

## Q9) How to create a database backup for Postgres and after to restore database from it

1) docker exec -t helpdesk-database-remote pg_dump -c -U postgres helpdesk > /home/ext_test/helpdesk_backup/dump_`date +%d-%m-%Y"_"%H_%M_%S`.sql

-- On Windows (cmd)
2) cd c:\Program Files\PostgreSQL\15\bin\
3) psql.exe -U postgres -d computers -f "C:\Users\valde\Desktop\Dumps\dump_10-01-2023_00_00_01.sql"

-- Docker on Linux
4) cat /home/vladimir/helpdesk_backup/dump_23-12-2022_12_15_38.sql | docker exec -i helpdesk-database-remote psql -U postgres -d helpdesk

## Q10) How to install a free ssl certificate (Let'sEncrypt, *.pem) for ASP.NET Core version 5.0 and up?

<https://dev.to/___bn___/free-certified-ssl-certificate-in-asp-net-5-kestrel-application-kgn> (very useful article)
<https://stackoverflow.com/questions/60724704/asp-net-core-application-in-docker-over-https>

## Q11) How to use ssh identity file with key for an access to the remote machine?

<https://adamtheautomator.com/add-ssh-key-to-vs-code/>

## Q12) I cannot drop a postgres database. What should I do?

Before all you must try to close/kill all connections to that database. Try the next code snippet for that

REVOKE CONNECT ON DATABASE helpdesk FROM PUBLIC;
DROP DATABASE helpdesk;

## Q13) How to copy file to a docker container from a host machine properly?

docker cp /home/leo/Helpdesk.pdf 93e45e5e2c6a:./app/wwwroot/files/Helpdesk.pdf

## Q14) How to install Cisco VPN Client

<https://treeone.ru/ustanovka-cisco-vpn-client-na-windows-10/#page-content>

1) Remove if SonicWALL VPN Client & Cisco VPN client are installed
2) Install SonicWall first
3) Open the unpacked folder with Cisco VPN Client, run not an EXE file, but an MSI
4) Run regedit
5) In the HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CVirtA branch, find the DisplayName entry, right-click edit and enter the new value instead of the old one:

## Q15) How can restore backup from localhost docker?

1) Open dataGrip
2) Create database connection to localhost docker database (on port 25432 in my case)
3) In context menu choose "Import/Export" item and restore database from psql
4) Find path to psql.exe (C:/Program Files/PostgreSQL/15/bin/psql.exe in my case) and to your backup (C:\Users\valde\Desktop\Dumps\dump_10-01-2023_00_00_01.sql in my case)
5) Run restore

## Q16) How to get files from docker volume?

1) Go to vs code docker extension
2) Select VOLUMES accordion item
3) Find your docker volume (helpdesk_back_helpdesk-volume in my case)
4) Click right mouse button
5) In the context menu select "Inspect" item to get JSON file
6) Copy Mountpoint (/var/lib/docker/volumes/helpdesk_back_helpdesk-volume/_data in my case)
7) sudo ls -l /var/lib/docker/volumes/helpdesk_back_helpdesk-volume/_data to get files name\

## Q17) How to copy files from docker volume to another directory at remote server?

1) Get volume path (/var/lib/docker/volumes/helpdesk_back_helpdesk-volume/_data in my case)
2) Select destination path (/usr/helpdesk-volume in my case)
3) sudo cp -r -a /var/lib/docker/volumes/helpdesk_back_helpdesk-volume/_data/. /usr/helpdesk-volume

## Q18) How to reinstall postgres on Linux?

<https://askubuntu.com/questions/817868/how-do-i-reinstall-postgresql-9-5-on-ubuntu-xenial-16-04-1>

1) sudo apt-get --purge remove postgresql-*
2) sudo rm -Rf /etc/postgresql /var/lib/postgresql
3) sudo apt-get install postgresql

## Q19) How to add user after postgres install?

<https://stackoverflow.com/questions/12720967/how-can-i-change-a-postgresql-user-password>

1) Go to /etc/postgresql/9.1/main/pg_hba.conf
2) Find this string (local     all         all             peer)
3) Change on it (local     all         all             md5)
4) sudo service postgresql restart (optional, i did not do it)
5) sudo -u postgres psql -c "ALTER USER postgres PASSWORD '<new-password>';"

## Q20) How to kill process on port Linux?

<https://stackoverflow.com/questions/11583562/how-to-kill-a-process-running-on-particular-port-in-linux>

1) kill -9 $(lsof -t -i:8080)

## Q21) How to change instruction file at helpdesk?

1) get pdf file (Инструкция для пользования Helpdesk.pdf)
2) Create new guid (var guid = Guid.NewGuid().ToString("N"); (result: c7f82c174aae445aa4ebe1d4f3a54ace))
3) Union name of pdf file with guid (Инструкция для пользования Helpdesk.pdf.c7f82c174aae445aa4ebe1d4f3a54ace)
4) Throw file (Инструкция для пользования Helpdesk.pdf.c7f82c174aae445aa4ebe1d4f3a54ace) to temp folder (/var/tmp/Инструкция для пользования Helpdesk.pdf.c7f82c174aae445aa4ebe1d4f3a54ace for example at helpdesk production, at root folder)
5) Throw file at docker volume (sudo cp -r -a "/var/tmp/Инструкция для пользования Helpdesk.pdf.c7f82c174aae445aa4ebe1d4f3a54ace"  /var/lib/docker/volumes/helpdesk_back_helpdesk-volume/_data/.)
6) To create record at dataBase at file table with name: Инструкция для пользования Helpdesk.pdf and uid: c7f82c174aae445aa4ebe1d4f3a54ace)
7) Check result

## Q22) How to delete file by pattern?

<https://askubuntu.com/questions/43709/how-do-i-remove-all-files-that-match-a-pattern>

1) find . -name '*.docx.*' -delete

## Q23) How to rename file from terminal on Linux?

<https://www.freecodecamp.org/news/rename-file-linux-bash-command/>

1) mv [options] source_file destination_file
