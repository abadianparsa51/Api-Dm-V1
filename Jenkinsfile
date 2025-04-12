pipeline {
    agent any

    tools {
        // فرض می‌کنیم dotnet SDK قبلاً تو Jenkins نصب شده
        // اگر نه، باید دستی نصب بشه یا داخل container استفاده بشه
        dotnet 'dotnet-sdk-8.0'
    }

    environment {
        PROJECT_PATH = 'UserApi'
    }

    stages {
        stage('Restore') {
            steps {
                dir("${env.PROJECT_PATH}") {
                    sh 'dotnet restore'
                }
            }
        }

        stage('Build') {
            steps {
                dir("${env.PROJECT_PATH}") {
                    sh 'dotnet build --configuration Release'
                }
            }
        }

        stage('Test') {
            steps {
                dir("${env.PROJECT_PATH}") {
                    sh 'dotnet test'
                }
            }
        }

        stage('Publish') {
            steps {
                dir("${env.PROJECT_PATH}") {
                    sh 'dotnet publish --configuration Release --output ./publish'
                }
            }
        }
    }

    post {
        always {
            echo 'Pipeline finished!'
        }
    }
}
