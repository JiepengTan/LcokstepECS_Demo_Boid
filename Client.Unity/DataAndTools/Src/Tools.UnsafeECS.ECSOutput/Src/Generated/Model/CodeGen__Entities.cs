
//------------------------------------------------------------------------------    
// <auto-generated>                                                                 
//     This code was generated by Tools.MacroExpansion, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null. 
//     https://github.com/JiepengTan/LockstepEngine                                          
//     Changes to this file may cause incorrect behavior and will be lost if        
//     the code is regenerated.                                                     
// </auto-generated>                                                                
//------------------------------------------------------------------------------  

//Power by ME //src: https://github.com/JiepengTan/ME  

//#define DONT_USE_GENERATE_CODE                                                                 
                                                                                                 
using System.Linq;                                                                               
using Lockstep.Serialization;                                                                    
using System.Runtime.InteropServices;                                                            
using System.Runtime.CompilerServices;                                                            
using System;                                                                                    
using Lockstep.InternalUnsafeECS;                                                               
using System.Collections;                                                                        
using Lockstep.Math;                                                                             
using System.Collections.Generic;                                                                
using Lockstep.Logging;                                                                          
using Lockstep.Util;                                                                          
namespace Lockstep.UnsafeECS.Game {  

    [StructLayout(LayoutKind.Sequential, Pack = Define.PackSize)]
    public unsafe partial class __entities {

        internal unsafe NativeArray<T> GetAllEntityFiledAry<T,TEntity>(NativeEntityArray<TEntity> entityAry,int offset)
            where T: unmanaged
            where TEntity : unmanaged, IEntity
        {
            var entityCount = entityAry.CurEntityCount;
            var retAry = new NativeArray<T>(entityCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            int curCopyedCount = 0;
            if (entityCount > 0) {
                var dstPtr = retAry.GetPointer(0);
                var len = entityAry.Length;
                var srcPtr = entityAry._EntityAry.GetPointer(0);
                for(int i =0;i < len; ++i,++srcPtr){
                    if(((Entity*) srcPtr)->_active){
                        #if DEBUG
                        if(++curCopyedCount > entityCount){
                            throw new Exception("FrameWork error! CurEntityCount !=  active Entity count");
                        }
                        #endif
                        *dstPtr = *((T*)((byte*)(srcPtr) + offset));
                        ++dstPtr;
                    }
                }
            }
            return retAry;
        }
        internal unsafe NativeArray<T> GetAllEntityFiledAry<T,TEntity>(
            NativeEntityArray<TEntity> entityAry,
            int offset,
            FuncEntityFilter<Entity> filterFunc,
            out int curCopyedCount
            )
            where T: unmanaged
            where TEntity : unmanaged, IEntity
        {
            var entityCount = entityAry.CurEntityCount;
            var retAry = new NativeArray<T>(entityCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            curCopyedCount = 0;
            if (entityCount > 0) {
                var dstPtr = retAry.GetPointer(0);
                var len = entityAry.Length;
                var srcPtr = entityAry._EntityAry.GetPointer(0);
                for(int i =0;i < len; ++i,++srcPtr){
                    var ePtr = (Entity*) srcPtr;
                    if(ePtr->_active) {
                        if (filterFunc(ePtr)) {
                            ++curCopyedCount;
#if DEBUG
                            if(curCopyedCount > entityCount){
                                throw new Exception("FrameWork error! CurEntityCount !=  active Entity count");
                            }
#endif
                            *dstPtr = *((T*)((byte*)(srcPtr) + offset));
                            ++dstPtr;
                        }
                    }
                }
            }
            return retAry;
        }


#region Size Offset of Entity Filed
        private NativeArray<T> _GetAllBoidSpawner_Transform3D<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Transform() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidSpawner_Transform3D<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Transform()+ compFiledOffset);}
        private NativeArray<Transform3D> _GetAllBoidSpawner_Transform(){return GetAllEntityFiledAry<Transform3D,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Transform());} 
        private int _GetOffsetOfBoidSpawner_Transform(){ var tempObj =  new BoidSpawner(); BoidSpawner* ptr = &tempObj;var filedPtr = &(ptr->Transform);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidSpawner_Prefab<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Prefab() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidSpawner_Prefab<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Prefab()+ compFiledOffset);}
        private NativeArray<Prefab> _GetAllBoidSpawner_Prefab(){return GetAllEntityFiledAry<Prefab,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Prefab());} 
        private int _GetOffsetOfBoidSpawner_Prefab(){ var tempObj =  new BoidSpawner(); BoidSpawner* ptr = &tempObj;var filedPtr = &(ptr->Prefab);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidSpawner_SpawnData<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Spawn() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidSpawner_SpawnData<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Spawn()+ compFiledOffset);}
        private NativeArray<SpawnData> _GetAllBoidSpawner_Spawn(){return GetAllEntityFiledAry<SpawnData,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Spawn());} 
        private int _GetOffsetOfBoidSpawner_Spawn(){ var tempObj =  new BoidSpawner(); BoidSpawner* ptr = &tempObj;var filedPtr = &(ptr->Spawn);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidSpawner_AssetData<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_BoidPrefab() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidSpawner_AssetData<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_BoidPrefab()+ compFiledOffset);}
        private NativeArray<AssetData> _GetAllBoidSpawner_BoidPrefab(){return GetAllEntityFiledAry<AssetData,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_BoidPrefab());} 
        private int _GetOffsetOfBoidSpawner_BoidPrefab(){ var tempObj =  new BoidSpawner(); BoidSpawner* ptr = &tempObj;var filedPtr = &(ptr->BoidPrefab);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidSpawner_BoidSpawnerTag<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Tag() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidSpawner_BoidSpawnerTag<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Tag()+ compFiledOffset);}
        private NativeArray<BoidSpawnerTag> _GetAllBoidSpawner_Tag(){return GetAllEntityFiledAry<BoidSpawnerTag,BoidSpawner>(_BoidSpawnerAry,_GetOffsetOfBoidSpawner_Tag());} 
        private int _GetOffsetOfBoidSpawner_Tag(){ var tempObj =  new BoidSpawner(); BoidSpawner* ptr = &tempObj;var filedPtr = &(ptr->Tag);  return (int)((long) filedPtr - (long) ptr);        } 
        private NativeArray<T> _GetAllBoidCell_CellData<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidCell>(_BoidCellAry,_GetOffsetOfBoidCell_Cell() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidCell_CellData<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidCell>(_BoidCellAry,_GetOffsetOfBoidCell_Cell()+ compFiledOffset);}
        private NativeArray<CellData> _GetAllBoidCell_Cell(){return GetAllEntityFiledAry<CellData,BoidCell>(_BoidCellAry,_GetOffsetOfBoidCell_Cell());} 
        private int _GetOffsetOfBoidCell_Cell(){ var tempObj =  new BoidCell(); BoidCell* ptr = &tempObj;var filedPtr = &(ptr->Cell);  return (int)((long) filedPtr - (long) ptr);        } 
        private NativeArray<T> _GetAllBoid_Transform3D<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,Boid>(_BoidAry,_GetOffsetOfBoid_Transform() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoid_Transform3D<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,Boid>(_BoidAry,_GetOffsetOfBoid_Transform()+ compFiledOffset);}
        private NativeArray<Transform3D> _GetAllBoid_Transform(){return GetAllEntityFiledAry<Transform3D,Boid>(_BoidAry,_GetOffsetOfBoid_Transform());} 
        private int _GetOffsetOfBoid_Transform(){ var tempObj =  new Boid(); Boid* ptr = &tempObj;var filedPtr = &(ptr->Transform);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoid_Prefab<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,Boid>(_BoidAry,_GetOffsetOfBoid_Prefab() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoid_Prefab<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,Boid>(_BoidAry,_GetOffsetOfBoid_Prefab()+ compFiledOffset);}
        private NativeArray<Prefab> _GetAllBoid_Prefab(){return GetAllEntityFiledAry<Prefab,Boid>(_BoidAry,_GetOffsetOfBoid_Prefab());} 
        private int _GetOffsetOfBoid_Prefab(){ var tempObj =  new Boid(); Boid* ptr = &tempObj;var filedPtr = &(ptr->Prefab);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoid_BoidState<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,Boid>(_BoidAry,_GetOffsetOfBoid_State() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoid_BoidState<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,Boid>(_BoidAry,_GetOffsetOfBoid_State()+ compFiledOffset);}
        private NativeArray<BoidState> _GetAllBoid_State(){return GetAllEntityFiledAry<BoidState,Boid>(_BoidAry,_GetOffsetOfBoid_State());} 
        private int _GetOffsetOfBoid_State(){ var tempObj =  new Boid(); Boid* ptr = &tempObj;var filedPtr = &(ptr->State);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoid_BoidTag<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,Boid>(_BoidAry,_GetOffsetOfBoid_Tag() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoid_BoidTag<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,Boid>(_BoidAry,_GetOffsetOfBoid_Tag()+ compFiledOffset);}
        private NativeArray<BoidTag> _GetAllBoid_Tag(){return GetAllEntityFiledAry<BoidTag,Boid>(_BoidAry,_GetOffsetOfBoid_Tag());} 
        private int _GetOffsetOfBoid_Tag(){ var tempObj =  new Boid(); Boid* ptr = &tempObj;var filedPtr = &(ptr->Tag);  return (int)((long) filedPtr - (long) ptr);        } 
        private NativeArray<T> _GetAllBoidTarget_Transform3D<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Transform() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidTarget_Transform3D<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Transform()+ compFiledOffset);}
        private NativeArray<Transform3D> _GetAllBoidTarget_Transform(){return GetAllEntityFiledAry<Transform3D,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Transform());} 
        private int _GetOffsetOfBoidTarget_Transform(){ var tempObj =  new BoidTarget(); BoidTarget* ptr = &tempObj;var filedPtr = &(ptr->Transform);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidTarget_Prefab<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Prefab() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidTarget_Prefab<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Prefab()+ compFiledOffset);}
        private NativeArray<Prefab> _GetAllBoidTarget_Prefab(){return GetAllEntityFiledAry<Prefab,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Prefab());} 
        private int _GetOffsetOfBoidTarget_Prefab(){ var tempObj =  new BoidTarget(); BoidTarget* ptr = &tempObj;var filedPtr = &(ptr->Prefab);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidTarget_TargetMoveInfo<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_MoveInfo() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidTarget_TargetMoveInfo<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_MoveInfo()+ compFiledOffset);}
        private NativeArray<TargetMoveInfo> _GetAllBoidTarget_MoveInfo(){return GetAllEntityFiledAry<TargetMoveInfo,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_MoveInfo());} 
        private int _GetOffsetOfBoidTarget_MoveInfo(){ var tempObj =  new BoidTarget(); BoidTarget* ptr = &tempObj;var filedPtr = &(ptr->MoveInfo);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidTarget_BoidTargetTag<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Tag() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidTarget_BoidTargetTag<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Tag()+ compFiledOffset);}
        private NativeArray<BoidTargetTag> _GetAllBoidTarget_Tag(){return GetAllEntityFiledAry<BoidTargetTag,BoidTarget>(_BoidTargetAry,_GetOffsetOfBoidTarget_Tag());} 
        private int _GetOffsetOfBoidTarget_Tag(){ var tempObj =  new BoidTarget(); BoidTarget* ptr = &tempObj;var filedPtr = &(ptr->Tag);  return (int)((long) filedPtr - (long) ptr);        } 
        private NativeArray<T> _GetAllBoidObstacle_Transform3D<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Transform() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidObstacle_Transform3D<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Transform()+ compFiledOffset);}
        private NativeArray<Transform3D> _GetAllBoidObstacle_Transform(){return GetAllEntityFiledAry<Transform3D,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Transform());} 
        private int _GetOffsetOfBoidObstacle_Transform(){ var tempObj =  new BoidObstacle(); BoidObstacle* ptr = &tempObj;var filedPtr = &(ptr->Transform);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidObstacle_Prefab<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Prefab() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidObstacle_Prefab<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Prefab()+ compFiledOffset);}
        private NativeArray<Prefab> _GetAllBoidObstacle_Prefab(){return GetAllEntityFiledAry<Prefab,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Prefab());} 
        private int _GetOffsetOfBoidObstacle_Prefab(){ var tempObj =  new BoidObstacle(); BoidObstacle* ptr = &tempObj;var filedPtr = &(ptr->Prefab);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidObstacle_PlayerData<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Player() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidObstacle_PlayerData<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Player()+ compFiledOffset);}
        private NativeArray<PlayerData> _GetAllBoidObstacle_Player(){return GetAllEntityFiledAry<PlayerData,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Player());} 
        private int _GetOffsetOfBoidObstacle_Player(){ var tempObj =  new BoidObstacle(); BoidObstacle* ptr = &tempObj;var filedPtr = &(ptr->Player);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidObstacle_SkillData<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Skill() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidObstacle_SkillData<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Skill()+ compFiledOffset);}
        private NativeArray<SkillData> _GetAllBoidObstacle_Skill(){return GetAllEntityFiledAry<SkillData,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Skill());} 
        private int _GetOffsetOfBoidObstacle_Skill(){ var tempObj =  new BoidObstacle(); BoidObstacle* ptr = &tempObj;var filedPtr = &(ptr->Skill);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidObstacle_MoveData<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Move() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidObstacle_MoveData<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Move()+ compFiledOffset);}
        private NativeArray<MoveData> _GetAllBoidObstacle_Move(){return GetAllEntityFiledAry<MoveData,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Move());} 
        private int _GetOffsetOfBoidObstacle_Move(){ var tempObj =  new BoidObstacle(); BoidObstacle* ptr = &tempObj;var filedPtr = &(ptr->Move);  return (int)((long) filedPtr - (long) ptr);        }
        private NativeArray<T> _GetAllBoidObstacle_BoidObstacleTag<T>(int compFiledOffset,FuncEntityFilter<Entity> filterFunc,out int length) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Tag() + compFiledOffset,filterFunc,out length);}
        private NativeArray<T> _GetAllBoidObstacle_BoidObstacleTag<T>(int compFiledOffset) where T: unmanaged{return GetAllEntityFiledAry<T,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Tag()+ compFiledOffset);}
        private NativeArray<BoidObstacleTag> _GetAllBoidObstacle_Tag(){return GetAllEntityFiledAry<BoidObstacleTag,BoidObstacle>(_BoidObstacleAry,_GetOffsetOfBoidObstacle_Tag());} 
        private int _GetOffsetOfBoidObstacle_Tag(){ var tempObj =  new BoidObstacle(); BoidObstacle* ptr = &tempObj;var filedPtr = &(ptr->Tag);  return (int)((long) filedPtr - (long) ptr);        }        

        private int _GetOffsetOfAnimator_Pad(){var tempObj =  new Animator(); Animator* ptr = &tempObj; var filedPtr = &(ptr->Pad);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfAnimator_Pad(){ return sizeof(int); } 
        private int _GetOffsetOfCollisionAgent_Collider(){var tempObj =  new CollisionAgent(); CollisionAgent* ptr = &tempObj; var filedPtr = &(ptr->Collider);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCollisionAgent_Collider(){ return sizeof(CollisionShape); }
        private int _GetOffsetOfCollisionAgent_IsTrigger(){var tempObj =  new CollisionAgent(); CollisionAgent* ptr = &tempObj; var filedPtr = &(ptr->IsTrigger);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCollisionAgent_IsTrigger(){ return sizeof(bool); }
        private int _GetOffsetOfCollisionAgent_Layer(){var tempObj =  new CollisionAgent(); CollisionAgent* ptr = &tempObj; var filedPtr = &(ptr->Layer);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCollisionAgent_Layer(){ return sizeof(int); }
        private int _GetOffsetOfCollisionAgent_IsEnable(){var tempObj =  new CollisionAgent(); CollisionAgent* ptr = &tempObj; var filedPtr = &(ptr->IsEnable);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCollisionAgent_IsEnable(){ return sizeof(bool); }
        private int _GetOffsetOfCollisionAgent_IsSleep(){var tempObj =  new CollisionAgent(); CollisionAgent* ptr = &tempObj; var filedPtr = &(ptr->IsSleep);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCollisionAgent_IsSleep(){ return sizeof(bool); }
        private int _GetOffsetOfCollisionAgent_Mass(){var tempObj =  new CollisionAgent(); CollisionAgent* ptr = &tempObj; var filedPtr = &(ptr->Mass);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCollisionAgent_Mass(){ return sizeof(LFloat); }
        private int _GetOffsetOfCollisionAgent_AngularSpeed(){var tempObj =  new CollisionAgent(); CollisionAgent* ptr = &tempObj; var filedPtr = &(ptr->AngularSpeed);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCollisionAgent_AngularSpeed(){ return sizeof(LFloat); }
        private int _GetOffsetOfCollisionAgent_Speed(){var tempObj =  new CollisionAgent(); CollisionAgent* ptr = &tempObj; var filedPtr = &(ptr->Speed);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCollisionAgent_Speed(){ return sizeof(LVector3); } 
        private int _GetOffsetOfNavMeshAgent_Pad(){var tempObj =  new NavMeshAgent(); NavMeshAgent* ptr = &tempObj; var filedPtr = &(ptr->Pad);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfNavMeshAgent_Pad(){ return sizeof(int); } 
        private int _GetOffsetOfPrefab_AssetId(){var tempObj =  new Prefab(); Prefab* ptr = &tempObj; var filedPtr = &(ptr->AssetId);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfPrefab_AssetId(){ return sizeof(int); } 
        private int _GetOffsetOfTransform2D_Position(){var tempObj =  new Transform2D(); Transform2D* ptr = &tempObj; var filedPtr = &(ptr->Position);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTransform2D_Position(){ return sizeof(LVector2); }
        private int _GetOffsetOfTransform2D_Deg(){var tempObj =  new Transform2D(); Transform2D* ptr = &tempObj; var filedPtr = &(ptr->Deg);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTransform2D_Deg(){ return sizeof(LFloat); }
        private int _GetOffsetOfTransform2D_Scale(){var tempObj =  new Transform2D(); Transform2D* ptr = &tempObj; var filedPtr = &(ptr->Scale);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTransform2D_Scale(){ return sizeof(LFloat); } 
        private int _GetOffsetOfTransform3D_Position(){var tempObj =  new Transform3D(); Transform3D* ptr = &tempObj; var filedPtr = &(ptr->Position);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTransform3D_Position(){ return sizeof(LVector3); }
        private int _GetOffsetOfTransform3D_Forward(){var tempObj =  new Transform3D(); Transform3D* ptr = &tempObj; var filedPtr = &(ptr->Forward);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTransform3D_Forward(){ return sizeof(LVector3); }
        private int _GetOffsetOfTransform3D_Scale(){var tempObj =  new Transform3D(); Transform3D* ptr = &tempObj; var filedPtr = &(ptr->Scale);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTransform3D_Scale(){ return sizeof(LFloat); } 
        private int _GetOffsetOfBoidSpawnerTag_Pad(){var tempObj =  new BoidSpawnerTag(); BoidSpawnerTag* ptr = &tempObj; var filedPtr = &(ptr->Pad);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfBoidSpawnerTag_Pad(){ return sizeof(int); } 
        private int _GetOffsetOfBoidTag_Pad(){var tempObj =  new BoidTag(); BoidTag* ptr = &tempObj; var filedPtr = &(ptr->Pad);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfBoidTag_Pad(){ return sizeof(int); } 
        private int _GetOffsetOfBoidObstacleTag_Pad(){var tempObj =  new BoidObstacleTag(); BoidObstacleTag* ptr = &tempObj; var filedPtr = &(ptr->Pad);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfBoidObstacleTag_Pad(){ return sizeof(int); } 
        private int _GetOffsetOfBoidTargetTag_Pad(){var tempObj =  new BoidTargetTag(); BoidTargetTag* ptr = &tempObj; var filedPtr = &(ptr->Pad);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfBoidTargetTag_Pad(){ return sizeof(int); } 
        private int _GetOffsetOfTargetMoveInfo_InitPos(){var tempObj =  new TargetMoveInfo(); TargetMoveInfo* ptr = &tempObj; var filedPtr = &(ptr->InitPos);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTargetMoveInfo_InitPos(){ return sizeof(LVector3); }
        private int _GetOffsetOfTargetMoveInfo_Interval(){var tempObj =  new TargetMoveInfo(); TargetMoveInfo* ptr = &tempObj; var filedPtr = &(ptr->Interval);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTargetMoveInfo_Interval(){ return sizeof(LFloat); }
        private int _GetOffsetOfTargetMoveInfo_Radius(){var tempObj =  new TargetMoveInfo(); TargetMoveInfo* ptr = &tempObj; var filedPtr = &(ptr->Radius);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTargetMoveInfo_Radius(){ return sizeof(LFloat); }
        private int _GetOffsetOfTargetMoveInfo_InitDeg(){var tempObj =  new TargetMoveInfo(); TargetMoveInfo* ptr = &tempObj; var filedPtr = &(ptr->InitDeg);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfTargetMoveInfo_InitDeg(){ return sizeof(LFloat); } 
        private int _GetOffsetOfBoidState_SinkTimer(){var tempObj =  new BoidState(); BoidState* ptr = &tempObj; var filedPtr = &(ptr->SinkTimer);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfBoidState_SinkTimer(){ return sizeof(LFloat); }
        private int _GetOffsetOfBoidState_IsDied(){var tempObj =  new BoidState(); BoidState* ptr = &tempObj; var filedPtr = &(ptr->IsDied);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfBoidState_IsDied(){ return sizeof(bool); }
        private int _GetOffsetOfBoidState_IsScored(){var tempObj =  new BoidState(); BoidState* ptr = &tempObj; var filedPtr = &(ptr->IsScored);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfBoidState_IsScored(){ return sizeof(bool); }
        private int _GetOffsetOfBoidState_Killer(){var tempObj =  new BoidState(); BoidState* ptr = &tempObj; var filedPtr = &(ptr->Killer);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfBoidState_Killer(){ return sizeof(EntityRef); } 
        private int _GetOffsetOfViewData_ViewId(){var tempObj =  new ViewData(); ViewData* ptr = &tempObj; var filedPtr = &(ptr->ViewId);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfViewData_ViewId(){ return sizeof(int); } 
        private int _GetOffsetOfSpawnData_Count(){var tempObj =  new SpawnData(); SpawnData* ptr = &tempObj; var filedPtr = &(ptr->Count);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSpawnData_Count(){ return sizeof(int); }
        private int _GetOffsetOfSpawnData_Radius(){var tempObj =  new SpawnData(); SpawnData* ptr = &tempObj; var filedPtr = &(ptr->Radius);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSpawnData_Radius(){ return sizeof(LFloat); }
        private int _GetOffsetOfSpawnData_Position(){var tempObj =  new SpawnData(); SpawnData* ptr = &tempObj; var filedPtr = &(ptr->Position);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSpawnData_Position(){ return sizeof(LVector3); } 
        private int _GetOffsetOfAssetData_AssetId(){var tempObj =  new AssetData(); AssetData* ptr = &tempObj; var filedPtr = &(ptr->AssetId);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfAssetData_AssetId(){ return sizeof(int); } 
        private int _GetOffsetOfCellIndexData_Index(){var tempObj =  new CellIndexData(); CellIndexData* ptr = &tempObj; var filedPtr = &(ptr->Index);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCellIndexData_Index(){ return sizeof(int); } 
        private int _GetOffsetOfScaleData_Value(){var tempObj =  new ScaleData(); ScaleData* ptr = &tempObj; var filedPtr = &(ptr->Value);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfScaleData_Value(){ return sizeof(LVector3); } 
        private int _GetOffsetOfPlayerData_Score(){var tempObj =  new PlayerData(); PlayerData* ptr = &tempObj; var filedPtr = &(ptr->Score);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfPlayerData_Score(){ return sizeof(int); }
        private int _GetOffsetOfPlayerData_LocalId(){var tempObj =  new PlayerData(); PlayerData* ptr = &tempObj; var filedPtr = &(ptr->LocalId);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfPlayerData_LocalId(){ return sizeof(int); } 
        private int _GetOffsetOfSkillData_IsNeedFire(){var tempObj =  new SkillData(); SkillData* ptr = &tempObj; var filedPtr = &(ptr->IsNeedFire);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSkillData_IsNeedFire(){ return sizeof(bool); }
        private int _GetOffsetOfSkillData_IsFiring(){var tempObj =  new SkillData(); SkillData* ptr = &tempObj; var filedPtr = &(ptr->IsFiring);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSkillData_IsFiring(){ return sizeof(bool); }
        private int _GetOffsetOfSkillData_CD(){var tempObj =  new SkillData(); SkillData* ptr = &tempObj; var filedPtr = &(ptr->CD);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSkillData_CD(){ return sizeof(LFloat); }
        private int _GetOffsetOfSkillData_CdTimer(){var tempObj =  new SkillData(); SkillData* ptr = &tempObj; var filedPtr = &(ptr->CdTimer);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSkillData_CdTimer(){ return sizeof(LFloat); }
        private int _GetOffsetOfSkillData_Duration(){var tempObj =  new SkillData(); SkillData* ptr = &tempObj; var filedPtr = &(ptr->Duration);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSkillData_Duration(){ return sizeof(LFloat); }
        private int _GetOffsetOfSkillData_DurationTimer(){var tempObj =  new SkillData(); SkillData* ptr = &tempObj; var filedPtr = &(ptr->DurationTimer);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSkillData_DurationTimer(){ return sizeof(LFloat); }
        private int _GetOffsetOfSkillData_AtkRange(){var tempObj =  new SkillData(); SkillData* ptr = &tempObj; var filedPtr = &(ptr->AtkRange);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfSkillData_AtkRange(){ return sizeof(LFloat); } 
        private int _GetOffsetOfMoveData_MoveSpd(){var tempObj =  new MoveData(); MoveData* ptr = &tempObj; var filedPtr = &(ptr->MoveSpd);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfMoveData_MoveSpd(){ return sizeof(LFloat); }
        private int _GetOffsetOfMoveData_AcceleratedSpd(){var tempObj =  new MoveData(); MoveData* ptr = &tempObj; var filedPtr = &(ptr->AcceleratedSpd);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfMoveData_AcceleratedSpd(){ return sizeof(LFloat); }
        private int _GetOffsetOfMoveData_CurSpd(){var tempObj =  new MoveData(); MoveData* ptr = &tempObj; var filedPtr = &(ptr->CurSpd);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfMoveData_CurSpd(){ return sizeof(LFloat); }
        private int _GetOffsetOfMoveData_AngularSpd(){var tempObj =  new MoveData(); MoveData* ptr = &tempObj; var filedPtr = &(ptr->AngularSpd);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfMoveData_AngularSpd(){ return sizeof(LFloat); }
        private int _GetOffsetOfMoveData_DeltaDeg(){var tempObj =  new MoveData(); MoveData* ptr = &tempObj; var filedPtr = &(ptr->DeltaDeg);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfMoveData_DeltaDeg(){ return sizeof(LFloat); } 
        private int _GetOffsetOfCellData_Count(){var tempObj =  new CellData(); CellData* ptr = &tempObj; var filedPtr = &(ptr->Count);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCellData_Count(){ return sizeof(int); }
        private int _GetOffsetOfCellData_Alignment(){var tempObj =  new CellData(); CellData* ptr = &tempObj; var filedPtr = &(ptr->Alignment);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCellData_Alignment(){ return sizeof(LVector3); }
        private int _GetOffsetOfCellData_Separation(){var tempObj =  new CellData(); CellData* ptr = &tempObj; var filedPtr = &(ptr->Separation);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCellData_Separation(){ return sizeof(LVector3); }
        private int _GetOffsetOfCellData_ObstacleDistance(){var tempObj =  new CellData(); CellData* ptr = &tempObj; var filedPtr = &(ptr->ObstacleDistance);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCellData_ObstacleDistance(){ return sizeof(LFloat); }
        private int _GetOffsetOfCellData_ObstaclePositionIndex(){var tempObj =  new CellData(); CellData* ptr = &tempObj; var filedPtr = &(ptr->ObstaclePositionIndex);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCellData_ObstaclePositionIndex(){ return sizeof(int); }
        private int _GetOffsetOfCellData_TargetPositionIndex(){var tempObj =  new CellData(); CellData* ptr = &tempObj; var filedPtr = &(ptr->TargetPositionIndex);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCellData_TargetPositionIndex(){ return sizeof(int); }
        private int _GetOffsetOfCellData_Index(){var tempObj =  new CellData(); CellData* ptr = &tempObj; var filedPtr = &(ptr->Index);return (int)((long) filedPtr - (long) ptr);        }
        private int _GetSizeOfCellData_Index(){ return sizeof(int); }        

#endregion

        internal int CurTotalEntityCount =>
            0 
            + CurBoidSpawnerCount
            + CurBoidCellCount
            + CurBoidCount
            + CurBoidTargetCount
            + CurBoidObstacleCount            
            ;


    }
}                                                                                
                                                                                         