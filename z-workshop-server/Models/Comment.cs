using System;
using System.Collections.Generic;

namespace z_workshop_server.Models;

public partial class Comment
{
    public string CommentId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public bool Type { get; set; }

    public string ResponseOf { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string ProductId { get; set; } = null!;

    public virtual ICollection<Comment> InverseResponseOfNavigation { get; set; } =
        new List<Comment>();

    public virtual Product Product { get; set; } = null!;

    public virtual Comment ResponseOfNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
