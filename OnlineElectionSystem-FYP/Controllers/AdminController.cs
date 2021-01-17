using OnlineElectionSystem_FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineElectionSystem_FYP.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        ElectionCommissionDatabaseEntities db;
        //public static int userId;
        //public static string userType="Admin";
        public static Citizen user;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageUserProfiles()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                if (user.Type != "Admin")
                {
                    return Redirect("Dashboard");
                } ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;
                ViewBag.CitizenList = db.Citizens.ToList();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View("Error");
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }

        public ActionResult CreateUser()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                ViewBag.ConstituencyLocs = db.Constituency_Locations.ToList();
            }
            catch
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(Models.Citizen citizen)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                Citizen newUser = new Citizen();
                newUser = citizen;
                Constitution constitute = ConstituteAllocation(Int32.Parse(citizen.PermanentAreaAddress));
                newUser.ConstituencyId = constitute.Id;
                if (newUser.Type == "Admin")
                {
                    Admin ad = new Admin();
                    ad.Address = citizen.CurrentAddress;
                    ad.CNIC = citizen.CNIC;
                    ad.Name = citizen.Name;
                    ad.Password = citizen.Password;
                    ad.PermanentAddress = citizen.PermanentAddress;
                    db.Admins.Add(ad);
                }
                else if (newUser.Type == "Candidate")
                {
                    Candidate cd = new Candidate();
                    cd.DateOfBirth = citizen.DateOfBirth;
                    cd.CNIC = citizen.CNIC;
                    cd.CurrentAddress = citizen.CurrentAddress;
                    cd.Name = citizen.Name;
                    cd.PermanentAddress = citizen.PermanentAddress;
                    db.Candidates.Add(cd);
                }
                db.Citizens.Add(newUser);
                db.SaveChanges();
                return Redirect("ManageUserProfiles");
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                return View();
            }
            finally
            {
                db.Database.Connection.Close();
            }
        }

        public Constitution ConstituteAllocation(int PermanentAddressAreaId)
        {
            db = new ElectionCommissionDatabaseEntities();
            Constitution c = new Constitution();
            try
            {
                var cc = db.Constituency_Locations.Single(f => f.Id.Equals(PermanentAddressAreaId));
                c = db.Constitutions.SingleOrDefault(f=>f.Id == cc.ConstituencyId);
            }
            catch(Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return c;
        }

        //public ActionResult Dashboard()
        //{
        //    db = new ElectionCommissionDatabaseEntities();
        //    try
        //    {
        //        ViewBag.Username = user.Name;
        //        ViewBag.UserRole = user.Type;
        //        ViewBag.lastlogin = user.lastLogin;
        //    }
        //    catch (Exception e)
        //    {
        //        ViewBag.Error = e.Message;
        //        return Redirect("Error");
        //    }
        //    finally
        //    {
        //        db.Database.Connection.Close();
        //    }
        //    return View();
        //}

        public ActionResult DeleteUser(int id)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                var u = db.Citizens.SingleOrDefault(ff => ff.Id == id);
                if (u != null)
                {
                    db.Citizens.Remove(u);
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("ManageUserProfiles");
        }

        public ActionResult EditUser(int id)
        {
            db = new ElectionCommissionDatabaseEntities();

            try
            {
                ViewBag.ConstituencyLocs = db.Constituency_Locations.ToList();
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;
                var editUser = db.Citizens.Single(f => f.Id.Equals(id));
                ViewBag.gotUser = editUser;
            }
            catch
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult EditUser(Citizen editedUser)
        {
            db = new ElectionCommissionDatabaseEntities();

            try
            {
                if (ModelState.IsValid)
                {
                    var user = db.Citizens.Single(f => f.Id.Equals(editedUser.Id));
                    user.Name = editedUser.Name;
                    user.CNIC = editedUser.CNIC;
                    user.Password = editedUser.Password;
                    user.CurrentAddress = editedUser.CurrentAddress;
                    user.PermanentAddress = editedUser.PermanentAddress;
                    user.DateOfBirth = editedUser.DateOfBirth;
                    user.Type = editedUser.Type;
                    user.CurrentAreaAddress = editedUser.CurrentAreaAddress;
                    user.PermanentAreaAddress = editedUser.PermanentAreaAddress;

                    db.SaveChanges();
                }
            }
            catch
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("ManageUserProfiles");
        }

        public ActionResult VerifyVoter()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;
                ViewBag.CitizenList = db.Citizens.ToList();
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }

        [HttpPost]
        public ActionResult VerifyVoter(int id)
        {
            return View();
        }

        public ActionResult VerifyCandidate()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                var requests = db.Candidate_Requests.ToList();
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;
                List<Candidate_Requests> c = new List<Candidate_Requests>();

                foreach (Candidate_Requests req in requests)
                {
                    Citizen citizenValues = db.Citizens.Single(f => f.Id == req.CitizenId);
                    Constitution constitutionValues = db.Constitutions.Single(f => f.Id == req.ConstitutionId);
                    Candidate_Requests candidateReq = req;
                    candidateReq.Citizen = citizenValues;
                    candidateReq.Constitution = constitutionValues;
                    if(candidateReq.Decision!="Confirmed")
                    c.Add(candidateReq);
                }
                ViewBag.Requests = c;
            }
            catch
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }

        [HttpGet]
        public FileResult DownloadFile(int id)
        {
            db = new ElectionCommissionDatabaseEntities();
            var FileById = db.Candidate_Requests.SingleOrDefault(f => f.Id == id);
            if (FileById.Documents == null)
            {
                return null;
            }
            else
            {
                return File(FileById.Documents, "application/pdf", "DocumentForVerification.pdf");
            }
        }

        public ActionResult ViewVotes()
        {
            return View();
        }

        public ActionResult DeclineCandidate(int id, int UserId)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                var updateStatus = db.Candidate_Requests.Single(f => f.Id.Equals(id));
                db.Candidate_Requests.Remove(updateStatus);
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("VerifyCandidate");
        }

        public ActionResult ConfirmCandidate(int id,int UserId)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                var getUser = db.Citizens.Single(f => f.Id.Equals(UserId));
                var updateStatus = db.Candidate_Requests.Single(f => f.Id.Equals(id));
                getUser.Type = "Candidate";
                Candidate cand = new Candidate();
                cand.CNIC = getUser.CNIC;
                cand.ConstituteId = updateStatus.ConstitutionId;
                cand.CurrentAddress = getUser.CurrentAreaAddress;
                cand.DateOfBirth = cand.DateOfBirth;
                cand.Name = getUser.Name;
                cand.PermanentAddress = getUser.PermanentAreaAddress;
                cand.PoliticalParty = updateStatus.PoliticalParty;
                db.Candidates.Add(cand);
                updateStatus.Decision = "Confirmed";
                db.SaveChanges();
            }
            catch
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("VerifyCandidate");
        }

        public ActionResult AnalyseSentiments()
        {
            db = new ElectionCommissionDatabaseEntities();

            try
            {
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;

                var list = db.VoterSentiments.ToList();
                foreach (var x in list)
                {
                    var f = db.Citizens.SingleOrDefault(ff => ff.Id.Equals(x.VoterId));
                    x.VoterCNIC = f.CNIC;
                    var can = db.Candidates.SingleOrDefault(ff => ff.Id == x.CandidateId);
                    x.CandidateName = can.Name;
                    x.Constituency = db.Constitutions.SingleOrDefault(ff=>ff.Id==can.ConstituteId).ConstitutionCode;
                }
                ViewBag.SentimentList = list;
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }

        public ActionResult ManageCandidates()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;

                var x = db.Candidates.Join(db.Constitutions, p => p.ConstituteId, m => m.Id, (p, m) => new { Candidate = p, Constitution = m}).ToList();
                List<Candidate> candidateList = new List<Candidate>();
                for(int i=0;i<x.Count;i++){
                    Candidate c = new Candidate();
                    c = x[i].Candidate;
                    c.Constitution = x[i].Constitution;
                    candidateList.Add(c);
                }
                ViewBag.Candidates = candidateList;
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }

        public ActionResult CreatePoliticalParty()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePoliticalParty(String PartyName, HttpPostedFileBase PartyImage)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                PoliticalParty p = new PoliticalParty();
                p.Name = PartyName;
                if (PartyImage != null)
                {
                    PartyImage.SaveAs(HttpContext.Server.MapPath("~/Content/pictures/" + p.Name + ".png"));
                }
                db.PoliticalParties.Add(p);
                db.SaveChanges();
            }
            catch(Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("ManagePoliticalParties");
        }

        public ActionResult DeletePoliticalParty(int id)
        {
            db = new ElectionCommissionDatabaseEntities();
            PoliticalParty pp = db.PoliticalParties.Single(f => f.Id == id);
            db.PoliticalParties.Remove(pp);
            db.SaveChanges();
            return RedirectToAction("ManagePoliticalParties");
        }

        public ActionResult EditCandidate(Models.Candidate c)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                Candidate cd = db.Candidates.Single(f => f.Id == c.Id);
                cd.PoliticalParty = c.PoliticalParty;
                cd.ConstituteId = c.ConstituteId;
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("ManageCandidates");
        }

        [HttpGet]
        public ActionResult EditCandidate(int id)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                ViewBag.Candidate = db.Candidates.Single(f => f.Id == id);
                ViewBag.Constituencies = db.Constitutions.ToList();
                ViewBag.PoliticalParties = db.PoliticalParties.ToList();
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }

        public ActionResult DeleteCandidate(int id)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                Candidate x = db.Candidates.Single(f => f.Id == id);
                Citizen xx = db.Citizens.Single(f => f.CNIC == x.CNIC);
                xx.Type = "Voter";
                db.Candidates.Remove(x);
                db.SaveChanges();
            }
            catch(Exception e)
            { 
            
            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("ManageCandidates");
        }

        public ActionResult ManagePoliticalParties()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                ViewBag.Parties = db.PoliticalParties.ToList();
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }
    }
}
