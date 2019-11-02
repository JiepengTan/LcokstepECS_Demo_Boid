
using Lockstep.Math;
using Lockstep.Serialization;

namespace Lockstep.Game {
    public partial class PlayerInput : BaseFormater,IBaseComponent {
        public ushort deg;
        public ushort skillId;

        public override void Serialize(Serializer writer){
            writer.Write(deg);
            writer.Write(skillId);
        }

        public void Reset(){
            deg = 0;
            skillId = 0;
        }

        public override void Deserialize(Deserializer reader){
            deg = reader.ReadUInt16();
            skillId = reader.ReadUInt16();
        }

        public static PlayerInput Empty = new PlayerInput();
        public override bool Equals(object obj){
            if (ReferenceEquals(this,obj)) return true;
            var other = obj as PlayerInput;
            return Equals(other);
        }

        public bool Equals(PlayerInput other){
            if (other == null) return false;
            if (deg != other.deg) return false;
            if (skillId != other.skillId) return false;
            return true;
        }

        public PlayerInput Clone(){
            var tThis = this;
            return new PlayerInput() {
                skillId = tThis.skillId,
                deg = tThis.deg,
            };
        }
    }
}