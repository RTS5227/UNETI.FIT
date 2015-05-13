
This file explains how to use the ExcelExportHelper class. 

The hepler takes in file name, sheet name, and ICollection of data. The export class returns a FileStreamResult
which can be downloaded as the file. 

Below is an example of the call:

        [HttpPost]
        public ActionResult Index(YOUR OBJECT) 
        {
            string sheetName = "Users";

            string fileName = "MyListOfUsers";

//This is only here to build a collection of users. You would have a collection of the data to pass and can skip this.

            var user1 = new User();
            user1.Id=1;
            user1.Name="Jane";

            var user2 = new User();
            user2.Id=2;
            user2.Name="John";

            var users = new HashSet<User>();
            users.Add(user1);
            users.Add(user2);
			
//End skip section.

//Create a var for the FileStreamResult to be returned from the CreateSheet method call. This is the file that the client will 
//download.

            var file = new ExcelExportHelper<User>(fileName, sheetName, users).CreateSheet();

            return file; //Return the file for it to be downloaded. 
        }
