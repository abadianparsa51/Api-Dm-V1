pipeline {
    agent any

    tools {
        // اصلاح به استفاده از dotnetsdk به جای dotnet
        dotnetsdk 'dotnet-sdk-8.0'
    }

    stages {
        stage('Build') {
            steps {
                script {
                    // کامپایل کردن پروژه
                    sh 'dotnet build UserApi/UserApi.csproj'
                }
            }
        }
        
        stage('Test') {
            steps {
                script {
                    // اجرای تست‌ها
                    sh 'dotnet test UserApi/UserApi.csproj'
                }
            }
        }
        
        stage('Publish') {
            steps {
                script {
                    // انتشار پروژه
                    sh 'dotnet publish UserApi/UserApi.csproj -c Release -o ./publish'
                }
            }
        }
    }
}
