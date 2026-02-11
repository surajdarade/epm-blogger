using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPM_Blogger.Domains.Models;
public partial class Tag
{
    [Key]
    public int TagId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [ForeignKey("TagId")]
    [InverseProperty("Tags")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
