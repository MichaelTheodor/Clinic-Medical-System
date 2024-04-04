using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLINIC
{
    public class visit
    {
        public int visit_id;
        public KeyValuePair<int, string> patient, secretary, doctor;
        public DateTime date;
        public string reasons, diagnosis, notes;
        private int patient_id;
        private string patient_name;
        private int secretary_id;
        private string secretary_name;
        private int doctor_id;
        private string doctor_name;
/*
        public visit(int visit_id, int patient_id, string patient_name, int secretary_id, string secretary_name, int doctor_id, string doctor_name, DateTime date, string reasons, string notes)
        {
            this.visit_id = visit_id;
            this.patient_id = patient_id;
            this.patient_name = patient_name;
            this.secretary_id = secretary_id;
            this.secretary_name = secretary_name;
            this.doctor_id = doctor_id;
            this.doctor_name = doctor_name;
            this.date = date;
            this.reasons = reasons;
            this.notes = notes;
        }
*/
        public visit(int visit_id, int patient_id, string patient_name, int secretary_id, string secretary_name, int doctor_id, string doctor_name, DateTime date, string reasons, string diagnosis, string notes)
        {
            this.visit_id = visit_id;
            patient = new KeyValuePair<int, string>(patient_id, patient_name);
            secretary = new KeyValuePair<int, string>(secretary_id, secretary_name);
            doctor = new KeyValuePair<int, string>(doctor_id, doctor_name);
            this.date = date;
            this.reasons = reasons;
            this.diagnosis = diagnosis;
            this.notes = notes;
        }

        public override string ToString()
        {
            return visit_id.ToString() + " => " + date.ToString("yyyy-MM-dd");
        }
    }
}
