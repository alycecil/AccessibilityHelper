/* 
 * Api Documentation
 *
 * Api Documentation
 *
 * OpenAPI spec version: 1.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = IO.Swagger.Client.SwaggerDateConverter;

namespace IO.Swagger.Model
{
    /// <summary>
    /// Event
    /// </summary>
    [DataContract]
    public partial class Event :  IEquatable<Event>, IValidatableObject
    {
        /// <summary>
        /// Defines Action
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ActionEnum
        {
            
            /// <summary>
            /// Enum CheckStatus for value: checkStatus
            /// </summary>
            [EnumMember(Value = "checkStatus")]
            CheckStatus = 1,
            
            /// <summary>
            /// Enum CheckHpMana for value: checkHpMana
            /// </summary>
            [EnumMember(Value = "checkHpMana")]
            CheckHpMana = 2,
            
            /// <summary>
            /// Enum Idle for value: idle
            /// </summary>
            [EnumMember(Value = "idle")]
            Idle = 3,
            
            /// <summary>
            /// Enum Logout for value: logout
            /// </summary>
            [EnumMember(Value = "logout")]
            Logout = 4,
            
            /// <summary>
            /// Enum Login for value: login
            /// </summary>
            [EnumMember(Value = "login")]
            Login = 5,
            
            /// <summary>
            /// Enum Teleport for value: teleport
            /// </summary>
            [EnumMember(Value = "teleport")]
            Teleport = 6,
            
            /// <summary>
            /// Enum Move for value: move
            /// </summary>
            [EnumMember(Value = "move")]
            Move = 7,
            
            /// <summary>
            /// Enum Repair for value: repair
            /// </summary>
            [EnumMember(Value = "repair")]
            Repair = 8,
            
            /// <summary>
            /// Enum SellInventory for value: sellInventory
            /// </summary>
            [EnumMember(Value = "sellInventory")]
            SellInventory = 9,
            
            /// <summary>
            /// Enum CheckInventory for value: checkInventory
            /// </summary>
            [EnumMember(Value = "checkInventory")]
            CheckInventory = 10,
            
            /// <summary>
            /// Enum StashInventory for value: stashInventory
            /// </summary>
            [EnumMember(Value = "stashInventory")]
            StashInventory = 11,
            
            /// <summary>
            /// Enum CombatGuard for value: combatGuard
            /// </summary>
            [EnumMember(Value = "combatGuard")]
            CombatGuard = 12,
            
            /// <summary>
            /// Enum CombatAttack for value: combatAttack
            /// </summary>
            [EnumMember(Value = "combatAttack")]
            CombatAttack = 13,
            
            /// <summary>
            /// Enum CombatCast for value: combatCast
            /// </summary>
            [EnumMember(Value = "combatCast")]
            CombatCast = 14
        }

        /// <summary>
        /// Gets or Sets Action
        /// </summary>
        [DataMember(Name="action", EmitDefaultValue=false)]
        public ActionEnum? Action { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="action">action.</param>
        /// <param name="priority">priority.</param>
        /// <param name="source">source.</param>
        /// <param name="targets">targets.</param>
        public Event(string id = default(string), ActionEnum? action = default(ActionEnum?), int? priority = default(int?), Entity source = default(Entity), List<Entity> targets = default(List<Entity>))
        {
            this.Id = id;
            this.Action = action;
            this.Priority = priority;
            this.Source = source;
            this.Targets = targets;
        }
        
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="__id", EmitDefaultValue=false)]
        public string Id { get; set; }


        /// <summary>
        /// Gets or Sets Priority
        /// </summary>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or Sets Source
        /// </summary>
        [DataMember(Name="source", EmitDefaultValue=false)]
        public Entity Source { get; set; }

        /// <summary>
        /// Gets or Sets Targets
        /// </summary>
        [DataMember(Name="targets", EmitDefaultValue=false)]
        public List<Entity> Targets { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Event {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Action: ").Append(Action).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  Source: ").Append(Source).Append("\n");
            sb.Append("  Targets: ").Append(Targets).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as Event);
        }

        /// <summary>
        /// Returns true if Event instances are equal
        /// </summary>
        /// <param name="input">Instance of Event to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Event input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Action == input.Action ||
                    (this.Action != null &&
                    this.Action.Equals(input.Action))
                ) && 
                (
                    this.Priority == input.Priority ||
                    (this.Priority != null &&
                    this.Priority.Equals(input.Priority))
                ) && 
                (
                    this.Source == input.Source ||
                    (this.Source != null &&
                    this.Source.Equals(input.Source))
                ) && 
                (
                    this.Targets == input.Targets ||
                    this.Targets != null &&
                    this.Targets.SequenceEqual(input.Targets)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.Action != null)
                    hashCode = hashCode * 59 + this.Action.GetHashCode();
                if (this.Priority != null)
                    hashCode = hashCode * 59 + this.Priority.GetHashCode();
                if (this.Source != null)
                    hashCode = hashCode * 59 + this.Source.GetHashCode();
                if (this.Targets != null)
                    hashCode = hashCode * 59 + this.Targets.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
