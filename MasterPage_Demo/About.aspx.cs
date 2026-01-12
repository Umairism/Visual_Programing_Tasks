using System;
using System.Collections.Generic;

namespace MasterPage_Demo
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTeamMembers();
            }
        }

        private void LoadTeamMembers()
        {
            // Create sample team members
            List<TeamMember> teamMembers = new List<TeamMember>
            {
                new TeamMember
                {
                    Name = "John Smith",
                    Position = "CEO & Founder",
                    Description = "Visionary leader with 15+ years in technology."
                },
                new TeamMember
                {
                    Name = "Sarah Johnson",
                    Position = "CTO",
                    Description = "Expert in cloud architecture and AI solutions."
                },
                new TeamMember
                {
                    Name = "Michael Chen",
                    Position = "Lead Developer",
                    Description = "Full-stack developer specializing in .NET."
                },
                new TeamMember
                {
                    Name = "Emily Davis",
                    Position = "UX Designer",
                    Description = "Creative designer focused on user experience."
                }
            };

            rptTeam.DataSource = teamMembers;
            rptTeam.DataBind();
        }

        // Team Member class
        public class TeamMember
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public string Description { get; set; }
        }
    }
}
