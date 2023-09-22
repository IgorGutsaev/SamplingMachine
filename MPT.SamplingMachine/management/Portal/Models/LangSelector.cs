using Filuet.Infrastructure.Abstractions.Enums;
using Filuet.Infrastructure.Abstractions.Helpers;
using Microsoft.AspNetCore.Components;

namespace Portal.Models
{
    public class LangSelector
    {
        public Language Language { get; set; }
        public int? Index { get; set; }

        public override string ToString()
            => Index.HasValue ? $"{Language.GetName()} <b>#{Index.Value}</b>" : Language.GetName();

        public MarkupString AsMarkup() =>
            new MarkupString($"<div class='markup'>{(Index.HasValue ? $"{Language.GetName()} <b>#{Index.Value}</b>" : Language.GetName())}</div>");
    }
}
