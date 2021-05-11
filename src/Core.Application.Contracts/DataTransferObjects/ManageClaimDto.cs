namespace Core.Application.Contracts.DataTransferObjects
{
    public class ManageClaimDto
    {
        public ManageClaimDto()
        {
            
        }
        public ManageClaimDto(string type, string value, bool @checked)
        {
            this.Type = type;
            this.Value = value;
            this.Checked = @checked;
        }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
    }
}
