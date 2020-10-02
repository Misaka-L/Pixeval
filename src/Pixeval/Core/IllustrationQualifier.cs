// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Linq;
using Pixeval.Data.ViewModel;

namespace Pixeval.Core
{
    public class IllustrationQualifier : IQualifier<Illustration, IllustrationQualification>
    {
        public bool Qualified(Illustration condition, IllustrationQualification pattern)
        {
            return pattern switch
            {
                { Type: ConditionType.Id }         => !condition.Id.Contains(pattern.Condition),
                { Type: ConditionType.Tag }        => !condition.Title.Contains(pattern.Condition) && !(condition.Tags != null && condition.Tags.Any(tag => tag?.Name != null && tag.Name.ToLower().Contains(pattern.Condition.ToLower()) || tag?.TranslatedName != null && tag.TranslatedName.ToLower().Contains(pattern.Condition.ToLower()))),
                { Type: ConditionType.ExcludeTag } => condition.Tags != null && condition.Tags.Any(tag => tag?.Name != null && tag.Name.ToLower().Contains(pattern.Condition[1..].ToLower()) || tag?.TranslatedName != null && tag.TranslatedName.ToLower().Contains(pattern.Condition[1..].ToLower())),
                _                                  => false
            };
        }
    }
}