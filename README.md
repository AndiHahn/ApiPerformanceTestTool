# ApiTester  

This tool can be used to test performance of an Web API  
Many Requests can be sent to an api  
Windows Desktop Application (WPF)  

## Setup
Setup for the request is similar to Postman, including setup for performance testing
#### Api Url  
Api url can be set manually including query parameters

#### Http Methods  
Available Http Methods: 
- Get  
- Put  
- Post  
- Head  
- Patch  

#### Performance Setup  
- Number of requests to be sent to given url  
- Requests per second to be sent.

#### Authentication  
Available authentication:  
- None
- Bearer token
- Credentials (username, password)

#### Request body
Request body can be set manually in json format

## Statistics  
All response Messages are shown in a list (Status code and Duration)  

#### Calculations  
- Average response time  
- Statistic of all different received status codes with counter  
- Response details (time sent, time received, duration, exception details (if any))
