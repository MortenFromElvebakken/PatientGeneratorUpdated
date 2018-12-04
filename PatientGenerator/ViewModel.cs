using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.Win32;
using PatientGenerator.Annotations;

namespace PatientGenerator
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            Initialize();
            ExecuteDefaultData(this);
            CreateCommands();
            ChangeBusyState(false);

        }
        //Added functionality for our use case
        //Initialize new fields for triage, specialty and ETA

        private void Initialize()
        {
            
            GenderList = new List<AdministrativeGender>()
            {
                AdministrativeGender.Male, AdministrativeGender.Female, AdministrativeGender.Other
                , AdministrativeGender.Unknown
            };
            ServerList = new List<string>()
            {
                "http://localhost:8080/hapi-fhir-jpaserver-example/baseDstu3",
                "http://10.192.154.107:8080/hapi-fhir-jpaserver-example/baseDstu3",
                "https://myfhirserver.azurewebsites.net/",
                "http://vonk.fire.ly/",
                "http://fhir.healthintersections.com.au/open",
                "http://spark.furore.com/fhir",
                "https://his-medicomp-gateway.orionhealth.com/blaze/fhir/",
                "http://hapi.fhir.org/baseDstu3"
            };
            TriageList = new List<string>()
            {
                "TriageRed",
                "TriageOrange",
                "TriageYellow",
                "TriageBlue",
                "TriageGreen",
                "anyUnknownString"
            };
            SpecialtyList = new List<string>()
            {
                "Diagnostic radiology",
                "Emergency medicine",
                "Neurology",
                "Obstetrics & Gynecology",
                "Opthalmology",
                "Orthopaedic surgery",
                "Otolaryngology",
                "Pathology-Anatomic & Clinical",
                "Pediatrics",
                "Plastic surgery",
                "Psychiatry",
                "Surgery-general",
                "Thoracic surgery",
                "Unknown",
                "Urology",
                "Vascular surgery"

            };
            HospitalList = new List<string>()
            {
                "AUH",
                "OUH"
            };

            // Complete list can be found at: http://www.englishclub.com/vocabulary/world-countries-nationality.htm
            NationalityList = new List<string>()
            {
                "Australian",
                "Austrian",
                "Belgian",
                "Bosnian",
                "British",
                "Bulgarian",
                "Canadian",
                "Cypriot",
                "Czech",
                "Danish",
                "Dutch",
                "English",
                "Estonian",
                "Finnish",
                "French",
                "Georgian",
                "German",
                "Greek",
                "Hungarian",
                "Irish",
                "Latvian",
                "Lithuanian",
                "Macedonian",
                "Maltese",
                "New Zealand",
                "Montenegrin",
                "Norwegian",
                "Polish",
                "Portuguese",
                "Romanian",
                "Russian",
                "Scottish",
                "Slovak",
                "Slovenian",
                "Swedish",
                "Swiss",
                "US",
                "Yugoslav",
                "Other"
            };


            MaritalStatusList = new List<string>()
            {
                "unknown", "divorced", "married", "never married", "widowed"
            };

            Url = ServerList.First();
        }

        private void NewPatient()
        {
            PatientId = "NEW PATIENT";
            Active = true;
            Deceased = false;

            Name = "";
            GivenName = "";
            BirthDate = DateTime.Now;
            //added:
            ETA = DateTime.Now;

            MultipleBirth = false;
            Gender = AdministrativeGender.Unknown;
            MaritalState = MaritalStatusList.FirstOrDefault(x => x.Equals("unknown"));
            Nationality = NationalityList.First(x => x.Equals("Other"));
            
            Address1 = "";
            Address2 = "";
            Zip = "";
            City = "";
            State = "";
            Country = "";
            CPR = "";
            FromDestination = "";
            AgeOfPatient = "";


            Phone = "";
            Mobile = "";
            Email = "";

            Photo = "unknown-male.jpg";
        }
        

        private void PopulateWithDefaultData()
        {
            NewPatient();

            Name = "Doe";
            GivenName = "John";
            BirthDate = new DateTime(1925, 8, 27);
            MultipleBirth = false;
            Gender = AdministrativeGender.Male;
            Active = true;
            Deceased = false;
            MaritalState = MaritalStatusList.FirstOrDefault(x => x.Equals("married"));
            Nationality = NationalityList.First(x => x.Equals("US"));

            Photo = "unknown-male.jpg";

            Address1 = "Nørre Alle";
            Address2 = "12";
            Triage = "TriageRed";
            Specialty = "Unknown";
            HospitalName = "AUH";
            CPR = "1212121212";
            FromDestination = "Grenåvej";
            AgeOfPatient = "40";
            Zip = "8000";
            City = "Aarhus";
            State = "Midtjylland";
            Country = "Denmark";

            Phone = "12345678";
            Mobile = "";
            Email = "dk@johndoeEmail.com";
        }

        #region lookup tables
        public List<AdministrativeGender> GenderList
        {
            get { return _genderList; }
            set
            {
                if (Equals(value, _genderList)) return;
                _genderList = value;
                OnPropertyChanged();
            }
        }
        public List<string> TriageList
        {
            get { return _triageList; }
            set
            {
                if (Equals(value, _triageList)) return;
                _triageList = value;
                OnPropertyChanged();
            }
        }
        public List<string> SpecialtyList
        {
            get { return _specialtyList; }
            set
            {
                if (Equals(value, _specialtyList)) return;
                _specialtyList = value;
                OnPropertyChanged();
            }
        }

        public List<string> HospitalList
        {
             get { return _hospitalList; }
             set
            {
                if (Equals(value, _hospitalList)) return;
                _hospitalList = value;
                OnPropertyChanged();
}
        }
        public List<string> ServerList
        {
            get { return _serverList; }
            set
            {
                if (Equals(value, _serverList)) return;
                _serverList = value;
                OnPropertyChanged();
            }
        }
        public List<string> NationalityList
        {
            get { return _nationalityList; }
            set
            {
                if (Equals(value, _nationalityList)) return;
                _nationalityList = value;
                OnPropertyChanged();
            }
        }
        public List<string> MaritalStatusList
        {
            get { return _maritalStatusList; }
            set
            {
                if (Equals(value, _maritalStatusList)) return;
                _maritalStatusList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region properties
        public string Url
        {
            get { return _url; }
            set
            {
                if (value == _url) return;
                _url = value;
                OnPropertyChanged();
            }
        }
        public string Status
        {
            get { return _status; }
            set
            {
                if (value == _status) return;
                _status = value;
                OnPropertyChanged();
            }
        }

        public bool SendImage
        {
            get { return _sendImage; }
            set
            {
                if (value.Equals(_sendImage)) return;
                _sendImage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region patient properties
        public string PatientId
        {
            get { return _id; }
            internal set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }
        public bool Active
        {
            get { return _active; }
            set
            {
                if (value.Equals(_active)) return;
                _active = value;
                OnPropertyChanged();
            }
        }
        public bool Deceased
        {
            get { return _deceased; }
            set
            {
                if (value.Equals(_deceased)) return;
                _deceased = value;
                OnPropertyChanged();
            }
        }
        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set
            {
                if (value.Equals(_birthDate)) return;
                _birthDate = value;
                OnPropertyChanged();
            }
        }
        public bool MultipleBirth
        {
            get { return _multipleBirth; }
            set
            {
                if (value.Equals(_multipleBirth)) return;
                _multipleBirth = value;
                OnPropertyChanged();
            }
        }
        public AdministrativeGender Gender
        {
            get { return _gender; }
            set
            {
                if (value == _gender) return;
                _gender = value;
                
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        public string GivenName
        {
            get { return _givenName; }
            set
            {
                if (value == _givenName) return;
                _givenName = value;
                OnPropertyChanged();
            }
        }
        public string MaritalState
        {
            get { return _marital; }
            set
            {
                if (value == _marital) return;
                _marital = value;
                OnPropertyChanged();
            }
        }
        public string Nationality
        {
            get { return _nationality; }
            set
            {
                if (value == _nationality) return;
                _nationality = value;
                OnPropertyChanged();
            }
        }
        public string HospitalName
        {
            get { return _hospitalName; }
            set
            {
                if (value == _hospitalName) return;
                _hospitalName = value;
                OnPropertyChanged();
            }
        }
        public string Triage
        {
            get { return _triage; }
            set
            {
                if (value == _triage) return;
                _triage = value;
                OnPropertyChanged();
            }
        }
        public string Specialty
        {
            get { return _specialty; }
            set
            {
                if (value == _specialty) return;
                _specialty = value;
                OnPropertyChanged();
            }
        }
        public DateTime ETA
        {
            get { return _eta; }
            set
            {
                if (value.Equals(_eta)) return;
                _eta = value;
                OnPropertyChanged();
            }
        }
        public string CPR
        {
            get { return _cpr; }
            set
            {
                if (value == _cpr) return;
                _cpr = value;
                OnPropertyChanged();
            }
        }

        public string AgeOfPatient
        {
            get { return _ageOfPatient; }
            set
            {
                if (value == _ageOfPatient) return;
                _ageOfPatient = value;
                OnPropertyChanged();
            }
        }

        public string FromDestination
        {
            get { return _fromDestination; }
            set
            {
                if (value == _fromDestination) return;
                _fromDestination = value;
                OnPropertyChanged();
            }
        }
        public string Address1
        {
            get { return _address1; }
            set
            {
                if (value == _address1) return;
                _address1 = value;
                OnPropertyChanged();
            }
        }
        public string Address2
        {
            get { return _address2; }
            set
            {
                if (value == _address2) return;
                _address2 = value;
                OnPropertyChanged();
            }
        }
        public string Zip
        {
            get { return _zip; }
            set
            {
                if (value == _zip) return;
                _zip = value;
                OnPropertyChanged();
            }
        }
        public string State
        {
            get { return _state; }
            set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                if (value == _city) return;
                _city = value;
                OnPropertyChanged();
            }
        }
        public string Country
        {
            get { return _country; }
            set
            {
                if (value == _country) return;
                _country = value;
                OnPropertyChanged();
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value == _phone) return;
                _phone = value;
                OnPropertyChanged();
            }
        }
        public string Mobile
        {
            get { return _mobile; }
            set
            {
                if (value == _mobile) return;
                _mobile = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged();
            }
        }
        public string Photo
        {
            get { return _photo; }
            set
            {
                if (value == _photo) return;
                _photo = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand CreatePatientCommand { get; internal set; }
        public ICommand UpdatePatientCommand { get; internal set; }

        public ICommand ResetDataCommand { get; internal set; }
        public ICommand DefaultDataCommand { get; internal set; }
        public ICommand ReadDataCommand { get; internal set; }
        public ICommand SelectImage { get; internal set; }


        private void CreateCommands()
        {
            CreatePatientCommand = new DelegateCommand(ExecuteCreateCommand, IsBusy);
            UpdatePatientCommand = new DelegateCommand(ExecuteUpdateCommand, IsBusy);

            ResetDataCommand = new DelegateCommand(ExecuteResetData, IsBusy);
            DefaultDataCommand = new DelegateCommand(ExecuteDefaultData, IsBusy);
            ReadDataCommand = new DelegateCommand(ExecuteReadData, IsBusy);
            SelectImage = new DelegateCommand(ShowImageSelctionDialog, IsBusy);
        }

        private void ExecuteReadData(object obj)
        {
            String id = PatientId;

            NewPatient();
            PatientId = "UNKNOWN";

            var uri = new Uri(_url);
            var client = new FhirClient(uri);



            var location = String.Format("{0}/Patient/{1}", _url, id);

            Patient entry = null;
            try
            {
                
                 entry = client.Read<Patient>(new Uri(location));
                
                //entry = client.Read(location);
                if (entry == null) return;

                _entry = entry;

                var identity = new ResourceIdentity(_entry.Id);
                PatientId = identity.Id;

                var patient = _entry;

                Active = patient.Active ?? true;
                var d = patient.Deceased as Hl7.Fhir.Model.FhirBoolean;
                if (d != null)
                {
                    Deceased = d.Value ?? false;
                }
                else
                {
                    Deceased = false;
                }

                Name = patient.Name[0].Family.ToString();
                GivenName = patient.Name[0].GivenElement[0].ToString();

                if (patient.BirthDate != null) BirthDate = DateTime.Parse(patient.BirthDate);

                var m = patient.MultipleBirth as FhirBoolean;
                if (m != null)
                {
                    MultipleBirth = m.Value ?? false;
                }
                else
                {
                    MultipleBirth = false;
                }


                // The following properties are not yet updated, simply set them to default values
                //
                // TODO
                // 
                Gender = patient.Gender ?? AdministrativeGender.Unknown;
                CPR = patient.Identifier[0].Value.ToString();
                Triage = patient.GetStringExtension("http://www.example.com/triagetest") ?? "Unknown";
                Specialty = patient.GetStringExtension("http://www.example.com/SpecialtyTest") ?? "Unknown";
                HospitalName = patient.Identifier[1].Value ?? "Unknown"; //patient.GetStringExtension("http://www.example.com/hospitalTest") ?? "To hospital went wrong";
                FromDestination = patient.Identifier[2].Value ?? "Unknown";
                //AgeOfPatient = patient.
                //Tjek op på at den er i Identity
                ETA = Convert.ToDateTime(patient.GetExtension("http://www.example.com/datetimeTest").Value.ToString());

                MaritalState = MaritalStatusList.FirstOrDefault(x => x.Equals("unknown"));
                Nationality = NationalityList.First(x => x.Equals("Other"));

               if(patient.Address[0] != null) Address1 = patient.Address[0].LineElement[0].ToString();
                if (patient.Address[0].LineElement[1] != null) Address2 = patient.Address[0].LineElement[1].ToString();

                if (patient.Address[0].PostalCode != null) Zip = patient.Address[0].PostalCode;
                if (patient.Address[0].City != null) City = patient.Address[0].City;
                if (patient.Address[0].State != null) State = patient.Address[0].State;
                if (patient.Address[0].Country != null) Country = patient.Address[0].Country;

                if (patient.Telecom[0] != null) Phone = patient.Telecom[0].Value;
                if (patient.Telecom[1] != null) Mobile = patient.Telecom[1].Value;
                if (patient.Telecom[2] != null) Email = patient.Telecom[2].Value;

                Photo = null;

            }
            catch (Exception)
            {
                NewPatient();
            }


        }

        private void ShowImageSelctionDialog(object obj)
        {

            var dialog = new OpenFileDialog();
            dialog.Title = "Select Image";
            dialog.Multiselect = false;

            var result = dialog.ShowDialog();
            if (result != null && result == true)
            {
                Photo = dialog.FileName;
                //SendImage = true;
            }
            else
            {
                //Photo = "unkown-female.jpg";
            }
        }

        private bool IsBusy(object obj)
        {
            return !_busy;
        }

        private void ExecuteCreateCommand(object obj)
        {
            InsertOrUpdatePatient(true);

        }

        private void ExecuteUpdateCommand(object obj)
        {
            InsertOrUpdatePatient(false);
        }


        private void ChangeBusyState(bool busy)
        {
            _busy = busy;
            ((DelegateCommand)CreatePatientCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)UpdatePatientCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ResetDataCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)DefaultDataCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ReadDataCommand).RaiseCanExecuteChanged();
        }

        private void ExecuteResetData(object obj)
        {
            //SendImage = false;
            NewPatient();
        }

        private void ExecuteDefaultData(object obj)
        {
            //SendImage = true;
            PopulateWithDefaultData();
        }


        private void InsertOrUpdatePatient(bool insert = true)
        {
            // Call into the model an pass the patient data
            ChangeBusyState(true);

            try
            {
                var uri = new Uri(_url);
                var client = new FhirClient(uri);
                
                var p = new Patient();
                p.Active = this.Active;

                if (!insert && _entry != null)
                {
                    p.Id = _entry.Id;
                    //p.Id = i;
                }
                String dob = BirthDate.Value.ToString("s");

                p.BirthDate = dob;
                p.Gender = Gender;
                var name = new HumanName();
                name.WithGiven(GivenName);
                name.AndFamily(Name);
                name.Text = GivenName + " " + Name;

                p.Name = new List<HumanName>();
                p.Name.Add(name);


                if (SendImage)
                {
                    p.Photo = new List<Attachment>();
                    var photo = new Attachment();
                    photo.Title = "Potrait - " + GivenName + " " + Name;

                    if (_photo.ToLower().EndsWith(".jpg"))
                    {
                        photo.ContentType = "image/jpeg";
                        using (FileStream fileStream = File.OpenRead(_photo))
                        {
                            int len = (int)fileStream.Length;

                            photo.Size = len;
                            photo.Data = new byte[fileStream.Length];
                            fileStream.Read(photo.Data, 0, len);
                        }
                        p.Photo.Add(photo);
                    }
                }

                p.Telecom = new List<ContactPoint>(3);
                p.FhirCommentsElement.Add(new FhirString("TEST AmbulanceNote"));
                var t = new ContactPoint();
                t.System = ContactPoint.ContactPointSystem.Phone;
                
                p.Telecom.Add(new ContactPoint()
                {
                    Value = Mobile,
                    System = ContactPoint.ContactPointSystem.Phone,
                    Use = ContactPoint.ContactPointUse.Mobile
                    
                });

                p.Telecom.Add(new ContactPoint()
                {
                    Value = Phone,
                    System = ContactPoint.ContactPointSystem.Phone,
                    Use = ContactPoint.ContactPointUse.Mobile
                });

                p.Telecom.Add(new ContactPoint()
                {
                    Value = Email,
                    System = ContactPoint.ContactPointSystem.Phone,
                    Use = ContactPoint.ContactPointUse.Mobile
                });

                p.Active = Active;
                p.MultipleBirth = new FhirBoolean(MultipleBirth);
                p.Deceased = new FhirBoolean(Deceased);

                //ToDo Marital Status

                p.Extension = new List<Extension>();
                //p.Extension.Add(new Extension(new Uri("http://www.englishclub.com/vocabulary/world-countries-nationality.htm"), new FhirString(Nationality)));
                //var ToHospitalName = new Extension("http://www.example.com/hospitalTest", new FhirString(HospitalName));
                //p.Extension.Add(ToHospitalName);
                p.Extension.Add(new Extension("http://www.example.com/triagetest", new FhirString(Triage)));
                p.Extension.Add(new Extension("http://www.example.com/SpecialtyTest", new FhirString(Specialty)));
                p.Extension.Add(new Extension("http://www.example.com/datetimeTest", new FhirDateTime(ETA)));

                
                //p.Identifier.Add(CPR);

                p.Identifier = new List<Identifier>(2);
                var cprIdentifier = new Identifier();
                cprIdentifier.Value = CPR;
                cprIdentifier.Use = Identifier.IdentifierUse.Official;
                cprIdentifier.System = "CPR";
                //cprIdentifier.Id = CPR;

                p.Identifier.Add(cprIdentifier);
                var toHospitalIdentifier = new Identifier();
                toHospitalIdentifier.Value = HospitalName;
                toHospitalIdentifier.Use = Identifier.IdentifierUse.Temp;
                toHospitalIdentifier.System = "ToHospital";
                toHospitalIdentifier.SystemElement = new FhirUri("http://www.example.com/hospitalTest");
                p.Identifier.Add(toHospitalIdentifier);


                var fromDestinationIdentifier = new Identifier();
                fromDestinationIdentifier.Value = FromDestination;
                fromDestinationIdentifier.Use = Identifier.IdentifierUse.Temp;
                fromDestinationIdentifier.System = "FromDestination";
                fromDestinationIdentifier.SystemElement = new FhirUri("http://www.example.com/fromdestination");
                p.Identifier.Add(fromDestinationIdentifier);

                p.Address = new List<Address>(1);
                var a = new Address();
                //a.zip = Zip;
                a.City = City;
                a.State = State;
                a.Country = Country;
                var lines = new List<String>();
                if (!String.IsNullOrEmpty(Address1)) lines.Add(Address1);
                if (!String.IsNullOrEmpty(Address2)) lines.Add(Address2);
                a.Line = lines;
                p.Address.Add(a);
                

                if (insert)
                {
                    client.PreferredFormat = ResourceFormat.Xml;
                    _entry = client.Create(p);
                }
                else
                {
                    _entry = p;
                    _entry = client.Update(_entry, true);
                }

                var identity = new ResourceIdentity(_entry.Id);
                PatientId = identity.Id;
                MessageBox.Show("Patient updated/created");
                Status = "Created new patient: " + PatientId;
                Debug.WriteLine(Status);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Status = e.Message;

            }
            finally
            {
                ChangeBusyState(false);
            }
        }
        #endregion

        #region private member variables
        private List<String> _serverList;
        private List<String> _nationalityList;

        private List<String> _triageList;
        private List<String> _specialtyList;
        private List<String> _hospitalList;
        private string _cpr;
        private string _fromDestination;


        private List<String> _maritalStatusList;
        private List<AdministrativeGender> _genderList;
        private bool _sendImage;

        private String _status;
        private bool _busy;

        private String _url;
        private String _id;
        private String _name;
        private String _givenName;
        private DateTime? _birthDate;
        private DateTime _eta;
        private bool _multipleBirth;
        private AdministrativeGender _gender;
        private String _marital;
        private String _nationality;
        private String _specialty;
        private String _triage;
        private String _hospitalName;
        private String _photo;
        private Patient _entry;
        private bool _active;
        private bool _deceased;

        private String _address1;
        private String _address2;
        private String _zip;
        private String _state;
        private String _city;
        private String _country;

        private String _phone;
        private String _mobile;
        private String _email;
        private string _ageOfPatient;

        #endregion
    }
}
