you must have docker desktop installed and running
to run you need vs code

assuming you already do it to run  do the following command in vs code terminal
1.docker start skyworx-redis
2.docker run -d --name skyworx-redis -p 6379:6379 redis:alpine
3.dotnet build
4.dotnet run --project KreditService

then to test go to http://localhost:5045/swagger/index.html
then to test do the following
1.go to POST /api/Auth/token
2.click try it out then click execute you will get the token then block it (just the token not including bearer)
3. then scroll up there is a padlock icon  that says authorize click it and copy paste the token.click authorize then click close
4. the authentication is authenticated
5. play with the rest of the app
6. at the create and put there are overide and proper prefix
7.overide just do the update/create without connecting it to calculate angsuran. if proper angsuran cannot be input because it will auto calculate
8. if you just want angsuran calculator POST /api/Kredit/calculate  no need to input id

i also have implemented serilog you can check example generated log at KreditService/Logs/there is a txt file here

i also made some test case
-Test_ShouldFail_WhenBungaExceedsOneHundredPercent
-Test_ShouldFail_WhenPlafonIsNegativeOrZero
-Test_ShouldPass_WhenInputsAreEntirelyValid
these are just to make sure that the inputs are valid so interest should not exceed 100% and plafon cannot be negative or 0 and combine them all

you can change the content in appsettings.json for the postgre i use default user postgres and for my pc the password is set as password just to make it simple
