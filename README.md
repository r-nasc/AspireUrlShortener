# URL Shortener with .NET Aspire

This project is a simple URL Shortener built to explore the capabilities of .NET Aspire.
While offering a straightforward concept, it still allows me to orchestrate various systems (FrontEnd, BackEnd, Databases, Cache, Auth..)

## Goals of the Project
The main objectives of this project were:
1. **Explore .NET Aspire's** – I wanted to assess how ready it is for production environments. My conclusion is that it is indeed production-ready, with ongoing improvements driven by community feedback.
2. **Experiment with .NET's HybridCache** – I wanted to try out .NET's new HybridCache and see how it performs in a real-world application.
3. **Learn React** – While FrontEnd development wasn’t my primary focus, I used this opportunity to try building a simple FrontEnd with React.

## Overview
### High Level Design
![design_overview](https://github.com/user-attachments/assets/93345c90-f376-4588-8913-b6ba3531a35c)
**Note**: Initially I also planned to incorporate user authentication, allowing individuals to view their own shortened URLs and enable/disable their links.
However, due to an unexpected drop in free time, this feature was never implemented.

### Orchestration
Orchestration is easily done, without need for dockerfiles and infrastucture management. 
Its service discovery proved robust, allowing seamless communication between services and great support for microservices architecture.
![image](https://github.com/user-attachments/assets/89b0713f-0bcc-4105-b035-a842924192d7)

![image](https://github.com/user-attachments/assets/0a54943c-a685-47c9-ae9a-02a7e56fb4a2)

### Instrumentation
Instrumentation is ready-to-use out-of-the-box, making it easy to monitor, measure, and analyze the performance of the different services. 
Aspire provides built-in tools for logging, tracing, and metrics collection, allowing developers to gain deep insights into application behavior and tracking events across microservices.
![image](https://github.com/user-attachments/assets/947f1650-140f-4fe2-9549-f0557e08d637)
![image](https://github.com/user-attachments/assets/d2d45a68-a49c-4039-99e2-69aa89c88d7a)

## Conclusion
.NET Aspire is a powerful and evolving framework that shows great potential for production environments. Especially with it's developers listening to community feedback and improving Aspire as a product on a daily basis.
