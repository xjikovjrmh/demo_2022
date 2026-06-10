# 对话记录 — 2026-06-10

## 会话摘要
用户要求在当前文件夹下的 `Claude` 文件夹内存放对话记录（在执行任务后更新）。
随后要求浏览项目内所有Scripts代码，熟悉项目框架。
随后讨论了存档系统（FacilityManager + FacilityData）的改进方案，方案已保存至 `Claude/改进方案.md`。
后续按模块拆分为独立文件：
- `Claude/模块-存档核心.md` — FacilityManager + FacilityData + FacilityIdentity 的待办与详解
- `Claude/模块-交互层.md` — Mouse_Touch 适配事件系统的待办与详解
- `Claude/问题汇总.md` — 实际开发中遇到的问题及其原理

## 当前项目状态
- **Unity 项目路径**: `D:\unityProject\demo_2022\demo_2022\`
- **当前分支**: `develop2`（主分支: `develop`）
- **Unity 版本**: Unity 2022
- **Git 用户**: b24042211@njupt.edu.cn

## 最近提交
1. `426d6f8` — 0.71_3
2. `56c816a` — 0.71_2
3. `6a107f8` — 0.71_1
4. `e5403be` — 0.71
5. `80282b0` — 0.70

## 当前修改的文件（未提交）
### 已修改
- `.gitignore`
- `Prefabs/tunnel.prefab`
- `Scenes/SampleScene.unity`
- `Scripts/Camera/CameraRotation.cs`
- `Scripts/HighLight/KeyBoardInput.cs`
- `Scripts/SaveTest/Facilities_Manager.cs`
- `Scripts/SaveTest/FacilityData.cs`
- `Scripts/SaveTest/PlayerData.cs`
- `Scripts/SaveTest/SaveSystem.cs`
- `ProjectSettings/TagManager.asset`

### 新增（未跟踪）
- `Scripts/SaveTest/FacilityIdentity.cs`
- `Scripts/SaveTest/FacilityIdentity.cs.meta`

## 正在开发的功能
- **存档系统**（Save System）: FacilityIdentity、Facilities_Manager、FacilityData、PlayerData、SaveSystem
- **摄像机旋转**（Camera Rotation）: CameraRotation.cs
- **键盘输入/高亮**（Keyboard Input/Highlighting）: KeyBoardInput.cs
- **隧道预制体**（Tunnel Prefab）
- **标签管理**（Tag Management）

## 项目架构总结

### 整体架构
该 Unity 项目是一个 3D 互动场景，包含**人物/车辆移动、摄像机系统、AB包资源管理、存档系统**等正在使用的模块，以及**事件系统、高亮交互、对象池**等学习实验性模块。

---

### 各模块实际状态

| 模块 | 状态 | 说明 |
|------|------|------|
| 存档系统 (SaveTest/) | ✅ **正在使用** | FacilityManager + FacilityData 是当前主要工作 |
| 资源加载 (SingletonAutoMono/) | ✅ 正在使用 | ABMgr 加载 AB 包资源 |
| 移动控制 (move/) | ✅ 正在使用 | PlayerMovement、CarMovement、CameraController |
| 摄像机系统 (Camera/) | ✅ 正在使用 | CameraRotation（第一人称/车辆/第三人称） |
| 菜单系统 (MenuList/StartProject) | ✅ 正在使用 | ESC 菜单和场景跳转 |
| 隧道生成 (GenerateTunnel/) | ⚠️ 实验/测试中 | AutoGenerateTunnel |
| 事件中心 (EventCenter/) | 📚 **学习阶段** | 发布-订阅模式，未确认使用场景 |
| 高亮交互 (HighLight/) | 📚 **学习阶段** | HighlightSystem2 + Mouse_Touch |
| 对象池 (bufferPool/) | 📚 **学习阶段** | PoolMgr，未确认使用场景 |

---

### ✅ 正式使用的模块

#### 1. 📦 资源加载 —— `SingletonAutoMono/`
| 文件 | 作用 |
|------|------|
| `SingletonAutoMono.cs` | **泛型单例基类** — 所有需要单例的 Mono 脚本可继承它，自动创建 GameObject 并挂载，过场景不销毁 |
| `ABMgr.cs` | **AssetBundle 管理器** — 继承单例，管理 AB 包的加载/卸载，支持同步/异步加载资源（泛型、Type、名称三种方式），自动处理依赖包和主包清单 |
| `ABTest.cs` | AB 加载测试脚本 |

#### 2. 🏃 移动控制 —— `move/`
| 文件 | 作用 |
|------|------|
| `PlayerMovement.cs` | **人物移动** — 用 Rigidbody 物理控制移动（WASD前后左右、Space上升、Ctrl下降、Shift加速） |
| `CarMovement.cs` | **车辆移动** — Rigidbody 控制，按 K 键切换自动前进 |
| `CameraController.cs` | **相机/车辆切换控制器** — 管理主视角、第一人称车辆视角、第三人称车辆视角三种模式切换，按 V 切换视角，按 B 切换目标车辆 |
| `ModeController.cs` | **（已注释）** 旧版模式切换控制器 |

#### 3. 📷 摄像机系统 —— `Camera/`
| 文件 | 作用 |
|------|------|
| `CameraRotation.cs` | **主摄像机（第一人称）** — 鼠标控制视角旋转，支持存档状态保存/加载 |
| `FrontCarCamera.cs` | **车辆第一人称视角** — 跟随车头位置 |
| `ThirdPersonCamera.cs` | **车辆第三人称视角** — 围绕目标旋转 |

#### 4. 💾 存档系统（当前主要工作） —— `SaveTest/`
| 文件 | 作用 | 当前焦点 |
|------|------|----------|
| `SaveSystem.cs` | **存档工具类（静态）** — 支持 PlayerPrefs 和 JSON 两种存档方式 | ✅ 基础工具 |
| `PlayerData.cs` | **玩家数据** — 保存/加载玩家位置、旋转、摄像机旋转状态 | ✅ 已完成 |
| **`FacilityManager.cs`** | **设施管理器** — 使用 GLTFast 加载 glb 模型，管理设施列表的增删改查，JSON 持久化 | ⭐ **当前工作重点** |
| **`FacilityData.cs`** | **设施数据模型** — 位置、旋转、缩放、ID 等信息 | ⭐ **当前工作重点** |
| `FacilityIdentity.cs` | **（已注释）** 设施 ID 组件 | 待定 |
| `Facilitied_test.cs` | 按 L 键添加测试设施 | 测试用 |
| `Test.cs` | JsonUtility 序列化测试 | 测试用 |

#### 5. 📋 菜单与启动
| 文件 | 作用 |
|------|------|
| `MenuList.cs` | ESC 菜单管理 — 暂停/恢复游戏、返回场景、重新开始、退出游戏 |
| `StartProject.cs` | 启动场景 → 加载下一个场景 |

---

### 📚 学习/实验中的模块（待确认使用场景）

#### 事件中心 —— `EventCenter/`
- **EventCenter** — 发布-订阅模式单例，通过枚举管理事件。目前仅 MonsterAction→TaskAction+playerAction 的 demo 级使用
- **待确认**：项目中哪些互动逻辑需要用事件解耦

#### 高亮交互 —— `HighLight/`
- **HighlightSystem2** — Outline 组件管理器，字典缓存
- **Mouse_Touch** — 鼠标射线选中/高亮/隐藏物体
- **待确认**：交互反馈以何种形式呈现

#### 对象池 —— `bufferPool/`
- **PoolMgr** — Dictionary+Stack 缓存池
- **DelayRemove** — 1s 延迟自动回池
- **待确认**：哪些频繁生成/销毁的对象需要池化

#### 隧道生成 —— `GenerateTunnel/`
- **AutoGenerateTunnel** — AB 包加载隧道预制体，H 键自动生成
- **待确认**：是否需要接入存档系统进行位置保存

---

### 🔄 当前工作聚焦
```
存档系统 (SaveTest/)
├── SaveSystem.cs         ✅ 存档基础工具（JSON/PlayerPrefs）
├── PlayerData.cs         ✅ 玩家数据存档（H保存/M加载）
├── FacilityManager.cs    ⭐ 正在处理 — 设施管理器
├── FacilityData.cs       ⭐ 正在处理 — 设施数据模型
├── FacilityIdentity.cs   ❌ 已注释 — 待决策
├── Facilitied_test.cs    🧪 测试脚本
└── Test.cs               🧪 序列化测试
```

## 用户环境
- **操作系统**: Windows 11 家庭中文版
- **Shell**: bash（Unix 语法）
- **平台**: win32
