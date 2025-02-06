using Microsoft.AspNetCore.Mvc.Rendering;


namespace RetailSales.Models
{
    public class Employee
    {
        public Employee()
        {
            //this.Materialstatuslst = new List<SelectListItem>();
            this.Countrylst = new List<SelectListItem>();
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();


        }
        //public List<SelectListItem> Materialstatuslst;
        public List<SelectListItem> Countrylst;
        public List<SelectListItem> Statelst;
        public List<SelectListItem> Citylst;
        public string ID { get; set; }
        public string EmpId { get; set; }
        public string Ename { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Cityid { get; set; }
        public string Stateid { get; set; }
        public string Countryid { get; set; }
        public string mobile { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
        public string Approved { get; set; }
        public string Designation { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Maritalstatus { get; set; }
        public string Emailpersonal { get; set; }
        public string Djoining { get; set; }
        public string Dbirth { get; set; }
        public string Dleaving { get; set; }
        public string Degree { get; set; }
        public string Bank { get; set; }
        public string Account { get; set; }
        public string Aadhar { get; set; }
        public string EmployeeStatus { get; set; }
        public string Report { get; set; }
        public string ddlStatus { get; set; }
      
        public string Qualification { get; set; }
        public string University { get; set; }
        public string School { get; set; }
        public string College { get; set; }
        public string Place { get; set; }
        public string YPassing { get; set; }
        public string Obedient { get; set; }

        public string Uname { get; set; }
        public string Pass { get; set; }

    }
    public class Employeegrid
    {
        public string id { get; set; }
        public string empid { get; set; }
        public string ename { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string cityid { get; set; }
        public string stateid { get; set; }
        public string countryid { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string remark { get; set; }
        public string approved { get; set; }
        public string designation { get; set; }
        public string branch { get; set; }
        public string department { get; set; }
        public string maritalstatus { get; set; }
        public string emailpersonal { get; set; }
        public string djoining { get; set; }
        public string dbirth { get; set; }
        public string dleaving { get; set; }
        public string degree { get; set; }
        public string employeeStatus { get; set; }
        public string report { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
     
    }
}
