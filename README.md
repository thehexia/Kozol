Közöl
=====

To set up, make sure you have VS2012 localdb SQL server instance installed.  Create a SQL login user by the name of "kozol" with password "kozol2014" for the localdb\v11.0 instance, and give it sysadmin rights.  Build the VS solution to download the NuGet packages, and generate the database from the Kozol.edmx.sql DDL script found in the models folder.  If the model is changed in the designer file, right click in the designer background and select to update the database from the model to regenerate the SQL script.  Make sure to select yes to include the credentials and uncheck the box asking to create a Web.config connection string.

Upon running the application for the first time, a couple sample users and the required user roles should be added to the database, and the Közöl chat demo should initialize and be visible with periodic incoming messages from 'Roundaround'.
