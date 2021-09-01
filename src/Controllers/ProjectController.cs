using ProjectA_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectA_MVC.Controllers
{
    public class ProjectController : Controller
    {
        //--------------------------------------------> Home Page <---------------------------------------------------------
        public ActionResult Index()
        {
            if(Session["role"]== null)
            {
                return View();
            }
            else
            {
                Session["role"] = null;
                return View();
            }
            
        }

        //--------------------------------------> Customer Login Part <---------------------------------------------------------
        public ActionResult CustomerLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CustomerLogin(ProjectA_MVC.Models.Customer_Tbl col)
        {
            try
            {
                using (project_DBEntities1 db = new project_DBEntities1())
                {
                    var detail = db.Customer_Tbl.Where(x => x.Email == col.Email && x.Password == col.Password).FirstOrDefault();
                    if (detail == null)
                    {
                        TempData["Message"] = "Invalid Login Credential";
                        return View("CustomerLogin");
                    }
                    else
                    {
                        Customer_Tbl tbl = db.Customer_Tbl.Single(x => x.Email == col.Email);
                        Session["role"] = "customer";
                        Session["id"] = tbl.Id;
                        Session["name"] = tbl.Name;
                        Session["address"] = tbl.Address;
                        Session["age"] = tbl.Age;
                        Session["password"] = tbl.Password;
                        Session["email"] = tbl.Email;
                        return RedirectToAction("Index1", "Project");
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("CustomerLogin");
            }
            
        }
        //--------------------------------------------> Executive Login Part <-------------------------------------------
        public ActionResult ExecutiveLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExecutiveLogin(ProjectA_MVC.Models.Executive_Tbl col)
        {
            try
            {
                using (project_DBEntities1 db = new project_DBEntities1())
                {
                    var detail = db.Executive_Tbl.Where(x => x.Email == col.Email && x.Password == col.Password).FirstOrDefault();
                    if (detail == null)
                    {
                        TempData["Message"] = "Invalid Login Credential";
                        return View("ExecutiveLogin");
                    }
                    else
                    {
                        Executive_Tbl tbl = db.Executive_Tbl.Single(x => x.Email == col.Email);
                        Session["role"] = "executive";
                        Session["name"] = tbl.Name;
                        Session["id"] = tbl.Id;
                        Session["email"] = tbl.Email;
                        return RedirectToAction("Index2", "Project");
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("ExecutiveLogin");
            }
            
        }

        //--------------------------------------------> Customer SignUp Part <-------------------------------------------
        public ActionResult CustomerSignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CustomerSignUp(ProjectA_MVC.Models.Customer_Tbl col)
        {
            try
            {
                using (project_DBEntities1 db = new project_DBEntities1())
                {
                    var detail = db.Customer_Tbl.Where(x => x.Email == col.Email).FirstOrDefault();
                    if (detail == null)
                    {
                        db.Customer_Tbl.Add(col);
                        db.SaveChanges();
                        TempData["Message"] = "Customer Registered Successfully";
                        ModelState.Clear();
                        return View("CustomerLogin");
                    }
                    else
                    {
                        TempData["Message"] = "Email Already Exist Please Login";
                        ModelState.Clear();
                        return View("CustomerLogin");
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("CustomerSignUp");
            }
            
        }

        //--------------------------------------------> Executive SignUp Part <-------------------------------------------
        public ActionResult ExecutiveSignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ExecutiveSignUp(ProjectA_MVC.Models.Executive_Tbl col)
        {
            try
            {
                using (project_DBEntities1 db = new project_DBEntities1())
                {
                    var detail = db.Executive_Tbl.Where(x => x.Email == col.Email).FirstOrDefault();
                    if (detail == null)
                    {
                        db.Executive_Tbl.Add(col);
                        db.SaveChanges();
                        ModelState.Clear();
                        TempData["Message"] = "Executive Registered Successfully";
                        return RedirectToAction("ExecutiveLogin", "Project");
                    }
                    else
                    {
                        TempData["Message"] = "Email Already Exist Please Login";
                        ModelState.Clear();
                        return View("ExecutiveLogin");
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("ExecutiveSignUp");
            }
            
            
        }
         
        //--------------------------------------------> Customer view after Successful Login <-------------------------------------------
        public ActionResult Index1()
        {
            return View();
        }


        //-------------------------------------------->Executive view after Successful Login <-------------------------------------------
        public ActionResult Index2()
        {
            return View();
        }
        //---------------------------------------> Customer's View Booking Page (after Successful Login) <------------------------------------
        public ActionResult ViewBooking(ProjectA_MVC.Models.Booking_Tbl col)
        {
            try
            {
                using (project_DBEntities1 db = new project_DBEntities1())
                {
                    var id = Convert.ToInt32(Session["id"]);
                    var data = db.Booking_Tbl.Where(d => d.Customer_Id == id).ToList();
                    return View(data);
                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("ViewBooking");
            }
            
        }
        //--------------------------------------------> Customer's Cancel Order Page (after Successful Login) <-------------------------------------------
        public ActionResult CancelOrder(int ID)
        {
            using (project_DBEntities1 db = new project_DBEntities1())
            {
                try
                {
                    var id2 = Convert.ToInt32(Session["id"]);
                    var id = db.Booking_Tbl.Where(d => d.Id == ID).First();
                    if (id == null)
                    {
                        TempData["Message"] = "Session Expired Please Login Again";
                        return View("CustomerLogin");
                    }
                    else
                    {
                        db.Booking_Tbl.Remove(id);
                        db.SaveChanges();
                        var data = db.Booking_Tbl.Where(d => d.Customer_Id == id2).ToList();
                        TempData["Message"] = "Order Cancelled";
                        return View("ViewBooking",data);
                    }
                }
                catch(Exception ex)
                {
                    TempData["Message"] = ex.Message;
                    return View("ViewBooking");
                }
               
            }
        }
        //--------------------------------------------> Executive Pending Delivery Screen <-------------------------------------------

        public ActionResult PendingDelivery(ProjectA_MVC.Models.Booking_Tbl col)
        {
            try
            {
                using (project_DBEntities1 db = new project_DBEntities1())
                {
                    //var id = Convert.ToInt32(Session["id"]);
                    // var data = db.Booking_Tbl.Where(d => d.Flag == true).ToList();
                    var data = db.Booking_Tbl.ToList();
                    return View(data);
                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("PendingDelivery");
            }
        }
        //--------------------------------------------> When Executive Marks an Order as Delivered  <-------------------------------------------
        public ActionResult DeliverOrder(Booking_Tbl obj)
        {
            try
            {
                using (project_DBEntities1 db = new project_DBEntities1())
                {
                    Booking_Tbl tbl = new Booking_Tbl();
                    if (ModelState.IsValid)
                    {
                        tbl.Id = obj.Id;
                        tbl.Customer_Id = obj.Customer_Id;
                        tbl.Date = obj.Date;
                        tbl.Time = obj.Time;
                        tbl.Weight = obj.Weight;
                        tbl.Price = obj.Price;
                        tbl.Flag = false;
                        db.Entry(tbl).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    ModelState.Clear();
                    //var data = db.Booking_Tbl.Where(d => d.Flag == true).ToList();
                    var data = db.Booking_Tbl.ToList();
                    TempData["Message"] = "Booking Delivered";
                    return View("PendingDelivery", data);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("PendingDelivery");
            }
        }

        //-----------------------------------------> Customer can Add Booking from this Page <-------------------------------------------
        public ActionResult AddBooking()
        {
            try
            {
                Booking_Tbl tbl = new Booking_Tbl
                {
                    Customer_Id = Convert.ToInt32(Session["id"])
                    //Price = 100
                };
                return View(tbl);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("AddBooking");
            }
        }
        [HttpPost]

        public ActionResult AddBooking(ProjectA_MVC.Models.Booking_Tbl col)
        {
            try
            {
                using (project_DBEntities1 db = new project_DBEntities1())
                {
                    db.Booking_Tbl.Add(col);
                    db.SaveChanges();
                }
                ModelState.Clear();
                TempData["Message"] = "Booking successfully added to the system";
                return RedirectToAction("ViewBooking", "Project");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View("AddBooking");
            }

        }
        //--------------------------------------------> Customer can view the Summary of their Order <-------------------------------------------
        public ActionResult Summary(ProjectA_MVC.Models.Booking_Tbl col)
        {
            try
            {
                Session["id"] = col.Customer_Id;
                Session["date"] = col.Date;
                Session["time"] = col.Time;
                
                Session["weight"] = col.Weight;
                Session["flag"] = col.Flag;
                Booking_Tbl tbl = new Booking_Tbl
                {
                    Customer_Id = Convert.ToInt32(Session["id"]),
                    Date = Session["date"].ToString(),
                    Time = Session["time"].ToString(),
                    
                    Weight = Convert.ToDouble(Session["weight"]),
                    Price = 100,
                    Flag = Convert.ToBoolean(Session["flag"])
                };
                if (col.Weight < 100)
                {
                    Session["price"] = 50;
                }
                else if(col.Weight < 200){
                    Session["price"] = 100;
                }
                else
                {
                    Session["price"] = 200;
                }
                return View();
                
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View();
            }
        }
     //-------------------------------------------->About Section of Application<-------------------------------------------
        public ActionResult About()
        {
            return View();
        }

        //-------------------------------------------->Terms Page of Application<-------------------------------------------
        public ActionResult Terms()
        {
            return View();
        }

        //-------------------------------------------->Change Address for customers<-------------------------------------------
        public ActionResult ChangeAddress()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeAddress(ProjectA_MVC.Models.Customer_Tbl col)
        {
            try
            {
                var data = new Customer_Tbl()
                {
                    
                    Id = Convert.ToInt32(col.Id),
                    Address = col.Address,
                    Name = Session["name"].ToString(),
                    Age = Convert.ToInt32(Session["age"]),
                    Password = Session["password"].ToString(),
                    ConfirPassword = Session["password"].ToString(),
                    Email = Session["email"].ToString()
                };
                using(project_DBEntities1 db = new project_DBEntities1())
                {
                    db.Customer_Tbl.Attach(data);
                    db.Entry(data).Property(x => x.Address).IsModified = true;
                    db.SaveChanges();
                    var address = col.Address;
                    TempData["Message"] = "Address Changed to '"+address+"'";
                    Session["address"] = data.Address;
                    return View("AddBooking");
                }
            }
                 catch(Exception ex)
                {
                    TempData["Message"] = ex.Message;
                    return View("AddBooking");
            }
        }
        //-------------------------------------------->Logout <-------------------------------------------
        public ActionResult Logout()
        {
            Session["id"] = null;
            Session["name"] = null;
            Session["address"] = null;
            Session["email"] = null;
            Session["age"] = null;
            Session["password"] = null;
            return View("Index");
        }
    }
}