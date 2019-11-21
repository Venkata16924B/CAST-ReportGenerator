﻿/*
 *   Copyright (c) 2019 CAST
 *
 * Licensed under a custom license, Version 1.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License, accessible in the main project
 * source code: Empowerment.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using System.Runtime.Serialization;

namespace CastReporting.Domain
{
    [DataContract]
    public abstract class DiagBase
    {

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "href")]
        public string HRef { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "key")]
        public int Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "number")]
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Annotation Annotation { get; set; }
    }
}
