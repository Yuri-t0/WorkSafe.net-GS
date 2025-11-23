using WorkSafe.Api.Domain.Enums;

namespace WorkSafe.Api.Domain.Entities
{
    public class Workstation
    {
         protected Workstation() { }

        public Workstation(
            string name,
            string employeeName,
            string department,
            int monitorDistanceCm,
            bool hasAdjustableChair,
            bool hasFootrest)
        {
            SetName(name);
            SetEmployeeName(employeeName);
            SetDepartment(department);
            SetMonitorDistance(monitorDistanceCm);

            HasAdjustableChair = hasAdjustableChair;
            HasFootrest = hasFootrest;
            LastEvaluationDate = DateTime.UtcNow;
            ErgonomicRiskLevel = CalculateRiskLevel();
        }

         public int Id { get; set; }

        public string Name { get; private set; } = string.Empty;
        public string EmployeeName { get; private set; } = string.Empty;
        public string Department { get; private set; } = string.Empty;
        public int MonitorDistanceCm { get; private set; }
        public bool HasAdjustableChair { get; private set; }
        public bool HasFootrest { get; private set; }
        public ErgonomicRiskLevel ErgonomicRiskLevel { get; private set; }
        public DateTime LastEvaluationDate { get; private set; }

        public bool IsCompliant => ErgonomicRiskLevel == ErgonomicRiskLevel.Low;

        public void Update(
            string name,
            string employeeName,
            string department,
            int monitorDistanceCm,
            bool hasAdjustableChair,
            bool hasFootrest)
        {
            SetName(name);
            SetEmployeeName(employeeName);
            SetDepartment(department);
            SetMonitorDistance(monitorDistanceCm);

            HasAdjustableChair = hasAdjustableChair;
            HasFootrest = hasFootrest;
            LastEvaluationDate = DateTime.UtcNow;
            ErgonomicRiskLevel = CalculateRiskLevel();
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            Name = name.Trim();
        }

        private void SetEmployeeName(string employeeName)
        {
            if (string.IsNullOrWhiteSpace(employeeName))
                throw new ArgumentException("Employee name is required.", nameof(employeeName));

            EmployeeName = employeeName.Trim();
        }

        private void SetDepartment(string department)
        {
            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department is required.", nameof(department));

            Department = department.Trim();
        }

        private void SetMonitorDistance(int monitorDistanceCm)
        {
            if (monitorDistanceCm <= 0)
                throw new ArgumentException("Monitor distance must be greater than 0.", nameof(monitorDistanceCm));

            MonitorDistanceCm = monitorDistanceCm;
        }

        private ErgonomicRiskLevel CalculateRiskLevel()
        {
            var riskScore = 0;

            if (MonitorDistanceCm < 40 || MonitorDistanceCm > 75)
                riskScore += 2;

            if (!HasAdjustableChair)
                riskScore += 2;

            if (!HasFootrest)
                riskScore += 1;

            return riskScore switch
            {
                <= 1 => ErgonomicRiskLevel.Low,
                2 or 3 => ErgonomicRiskLevel.Medium,
                _ => ErgonomicRiskLevel.High
            };
        }
    }
}
