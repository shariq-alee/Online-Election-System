using OnlineElectionSystem_FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineElectionSystem_FYP.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public static Citizen user;
        static ElectionCommissionDatabaseEntities db;

        public ActionResult Dashboard()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                ViewBag.Constitutencies = db.Constitutions.ToList();
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;
                ViewBag.loginlast = user.lastLogin;
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
                return Redirect("Error");
            }
            finally
            {
                db.Database.Connection.Close();
            }           
            return View();
        }
        [HttpGet]
        public ActionResult CastVote()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                //
                var xc = db.Constitutions.SingleOrDefault(f => f.Id == user.ConstituencyId);
                var vote = db.Election_Votes_Candidate.SingleOrDefault(f => f.VoterId == user.Id);
                ViewBag.Constituencies = xc;
                if (vote == null)
                {
                    ViewBag.Voted = "Not Voted";
                }
                else
                {
                    ViewBag.Voted = "Voted";
                }
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;
            }
            catch(Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return View();
        }

        [HttpGet]
        public ActionResult VoteFeedBack()
        {
            ViewBag.Username = user.Name;
            ViewBag.UserRole= user.Type;
            return View();
        }

        [HttpPost]
        public ActionResult VoteFeedBack(VoterSentiment vs)
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
                VoterSentiment newSentiment = vs;
                newSentiment.VoterId = user.Id;
                newSentiment.CandidateId = candId;
                db.VoterSentiments.Add(newSentiment);
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("Dashboard");
        }
        static int candId=0;
        public ActionResult CastVoteToCandidate(int ConstituteId, int CandidateId)
        {
            db = new ElectionCommissionDatabaseEntities();

            try
            {
                bool status = false;
                var x = db.Election_Votes_Candidate.SingleOrDefault(f => f.VoterId==user.Id);
                if (x == null)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                if (status)
                {
                    candId = CandidateId;
                    Election_Votes_Candidate vote = new Election_Votes_Candidate();
                    vote.CandidateId = CandidateId;
                    vote.ConstitutionId = ConstituteId;
                    vote.VoterId = user.Id;
                    db.Election_Votes_Candidate.Add(vote);
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
            return RedirectToAction("VoteFeedBack");
        }

        [HttpPost]
        public JsonResult getCandidates(int constituencyId)
        {
            //var c;
            db = new ElectionCommissionDatabaseEntities();
            List<Candidate> c = db.Candidates.Where(f => f.ConstituteId == constituencyId).ToList();
            foreach (var item in c)
            {
                Constitution cons = new Constitution();
                cons = db.Constitutions.Single(f => f.Id == item.ConstituteId);
                c.Single(f => f.Id.Equals(item.Id)).Constitution = cons;
            }
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCandidateVotes(int constituencyId)
        {
            db = new ElectionCommissionDatabaseEntities();
            List<Candidate> candidateList = new List<Candidate>();

            try
            {
                candidateList = db.Candidates.Where(f => f.ConstituteId == constituencyId).ToList();
                foreach (Candidate c in candidateList)
                {
                    int numOfVotes = db.Election_Votes_Candidate.Where(f => f.CandidateId == c.Id).ToList().Count;
                    c.numberOfVotes = numOfVotes;
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }

            return Json(candidateList,JsonRequestBehavior.AllowGet);
        }

        public ActionResult BecomeACandidate()
        {
            db = new ElectionCommissionDatabaseEntities();

            try
            {
                ViewBag.Constitutions = db.Constitutions.ToList();
                ViewBag.PoliticalParties = db.PoliticalParties.ToList();
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
        public ActionResult BecomeACandidate(String ConstitutionCode, String politicalParty, String RequestNote, HttpPostedFileBase image1)
        {
            db = new ElectionCommissionDatabaseEntities();

            try
            {
                var constitutionId = db.Constitutions.Single(f => f.ConstitutionCode == ConstitutionCode).Id;
                if (constitutionId != null)
                {
                    Candidate_Requests CandidateReq = new Candidate_Requests();
                    if (image1 != null)
                    {
                        CandidateReq.Documents = new byte[image1.ContentLength];
                        image1.InputStream.Read(CandidateReq.Documents, 0, image1.ContentLength);
                    }
                    CandidateReq.ConstitutionId = constitutionId;
                    CandidateReq.CitizenId = user.Id;
                    CandidateReq.RequestNote = RequestNote;
                    CandidateReq.PoliticalParty = politicalParty;
                    db.Candidate_Requests.Add(CandidateReq);
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
            return Redirect("Dashboard");
        }

        public ActionResult Settings()
        {
            db = new ElectionCommissionDatabaseEntities();

            try
            {
                ViewBag.ConstituencyLocs = db.Constituency_Locations.ToList();
                var getUser = db.Citizens.Single(f => f.Id.Equals(user.Id));
                ViewBag.User = getUser;
                ViewBag.Username = user.Name;
                ViewBag.UserRole = user.Type;
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
        public ActionResult EditProfileSettings(Citizen CitizenChanges)
        {
            db = new ElectionCommissionDatabaseEntities();

            try
            {
                var getUser = db.Citizens.Single(f => f.Id.Equals(user.Id));
                getUser.Name = CitizenChanges.Name;
                getUser.CNIC = CitizenChanges.CNIC;
                getUser.Password = CitizenChanges.Password;
                getUser.DateOfBirth = CitizenChanges.DateOfBirth;
                getUser.PermanentAddress = CitizenChanges.PermanentAddress;
                getUser.CurrentAddress = CitizenChanges.CurrentAddress;
                getUser.CurrentAreaAddress = CitizenChanges.CurrentAreaAddress;
                getUser.PermanentAreaAddress = CitizenChanges.PermanentAreaAddress;

                db.SaveChanges();
                user = getUser;
            }
            catch(Exception e)
            {

            }
            finally
            {
                db.Database.Connection.Close();
            }
            return RedirectToAction("Dashboard");
        }

        public ActionResult ResultReports()
        {
            db = new ElectionCommissionDatabaseEntities();
            try
            {
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

        public ActionResult getConstituencies()
        {
            db = new ElectionCommissionDatabaseEntities();
            List<Constitution> l = db.Constitutions.ToList();

            foreach (var x in l)
            {
                var totalCandidates = db.Candidates.Where(f => f.ConstituteId == x.Id).Count();
                var totalVoters = db.Citizens.Where(f => f.ConstituencyId == x.Id).Count();
                var totalVotesCasted = db.Election_Votes_Candidate.Where(f => f.ConstitutionId == x.Id).Count();
                x.TotalCandidates = totalCandidates;
                x.TotalNumberOfVoters = totalVoters;
                x.TotalVotesCasted = totalVotesCasted;
                //finding winner
                var cands = db.Candidates.Where(f => f.ConstituteId == x.Id).ToList();
                Candidate winner = null;
                for (int i = 0; i < cands.Count(); i++)
                {
                    int candidateID = cands[i].Id;
                    int? ConstituteID = cands[i].ConstituteId;
                    cands[i].numberOfVotes = db.Election_Votes_Candidate.Where(f=>f.CandidateId == candidateID && f.ConstitutionId == ConstituteID).Count();
                }
                x.Candidates = cands;
                //winner = cands.Max(r=>r.numberOfVotes);
            }

            return Json(l, JsonRequestBehavior.AllowGet);
        }
        //List<Candidate> c = db.Candidates.Where(f => f.ConstituteId == constituencyId).ToList();
        //    foreach (var item in c)
        //    {
        //        Constitution cons = new Constitution();
        //        cons = db.Constitutions.Single(f => f.Id == item.ConstituteId);
        //        c.Single(f => f.Id.Equals(item.Id)).Constitution = cons;
        //    }
        //    return Json(c, JsonRequestBehavior.AllowGet);
    }
}
