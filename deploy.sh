#!/usr/bin/env sh

# 发生错误时终止
set -e

# 进入构建文件夹
cd build

# 如果你要部署到自定义域名
# echo 'www.example.com' > CNAME

git init
git add .
git commit -m 'build'

# 如果你要部署在 https://<USERNAME>.github.io
# git push -f git@github.com:<USERNAME>/<USERNAME>.github.io.git main

# 如果你要部署在 https://<USERNAME>.github.io/<REPO>
git push -f git@github.com:mingxuann/pinkman-2d-unity.git master:github-page

cd -