# q-clipboard-server
Q Clipboard Server是一个跨平台的剪切板同步方案。

使用之前你需要一个服务器来部署服务。

## 服务器端

### docker部署

推荐使用docker来部署，docker-compose.yaml如下：

```yaml
version: '3'

services:
  q-clipboard-server:
    image: patrickwangqy/q-clipboard-server
    restart: unless-stopped
    volumes:
      - ./config:/app/config
    ports:
      - "5000:5000"
```

### 编译部署

1. 安装.net 6.0

    https://dotnet.microsoft.com/en-us/download/dotnet/6.0

2. 执行编译命令

```powershell
dotnet build -c Release
```

程序会生成在bin/Release/net6.0

3. 运行程序

## 客户端

本着不重复造轮子的思想，所有的客户端均使用已存在的app

### Windows

windows端使用powershell脚本来实现剪切板同步

[Windows客户端](Clients/Windows/q-clipboard-pc)

### Android

Android端使用[http-shortcuts](https://http-shortcuts.rmy.ch/)

TODO：更新教程

### iOS

iOS端使用快捷指令

TODO：更新教程

## 功能列表

[x] 文本推送、拉取

[x] 图像推送、拉取

[x] 文件推送、拉取

[] 剪切板历史
