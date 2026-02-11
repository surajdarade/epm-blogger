using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPM_Blogger.Domains.Models;

public partial class Post
{
    [Key]
    public int PostId { get; set; }

    public int UserId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Post")]
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    [ForeignKey("UserId")]
    [InverseProperty("Posts")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("PostId")]
    [InverseProperty("Posts")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
