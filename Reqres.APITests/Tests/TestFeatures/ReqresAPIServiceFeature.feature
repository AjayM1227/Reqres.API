Feature: ReqresAPIServiceFeature
	This feature file contains Reqres API Feature testcases related scenarios

Scenario Outline: GET Request to get list of users
Given I access the API endpoint "ReqresAPIGetListUsers" with <page>
When I Generate GET API request
Then I should be displayed with response <StatusCode> for API "ReqresAPIGetListUsers"
And user list response should contain FirstName <first_name>, LastName <last_name>, Email <email>, UserName <user_name>

Examples: 
| page | StatusCode | first_name | last_name | user_name        | email                      |
| 2    | 200        | Lindsay    | Ferguson  | Lindsay Ferguson | lindsay.ferguson@reqres.in |
| 3    | 201        | Lindsay    | Ferguson  | Lindsay Ferguson | lindsay.ferguson@reqres.in |
| 2    | 200        | user01     | user01    | Lindsay Ferguson | lindsay.ferguson@gmail.in |
| 2    | 200        | Janet      | Weaver    |Janet Weaver      |  janet.weaver@reqres.in| 
| 2    | 200        | Abhi     | Ram    | Abhi Ram | AbhiRam@gmail.in |



Scenario Outline: GET Request to retrieve single user
Given I access the API endpoint "ReqresAPIGetSingleUser" with <page>
When I Generate GET API request
Then I should be displayed with response <StatusCode> for API "ReqresAPIGetSingleUser"
And user list response should contain FirstName <first_name>, LastName <last_name>, Email <email>, UserName <user_name>

Examples: 
| page | StatusCode | first_name | last_name | email                  | user_name    |
| 2    | 200        | Janet      | Weaver    | janet.weaver@reqres.in | Janet Weaver | 
| 2    | 200        | Lindsay    | Ferguson  | Lindsay Ferguson | lindsay.ferguson@reqres.in |


Scenario Outline: Create single user with POST request 
Given I access the API endpoint "ReqresAPIPOSTCreateUser" with <page>
When I execute POST request for ReqresAPIPOSTCreateUser API
Then I should be displayed with response <StatusCode> for API "ReqresAPIPOSTCreateUser"

Examples: 
| page | StatusCode |
| 2   | 201        |
| 2   | 203        |


Scenario Outline: Update single user details with PUT request
Given I access the API endpoint "ReqresAPIPUTUpdateSingleUser" with <page>
When I Generate PUT API request for ReqresAPIPUTUpdateSingleUser with Name <name>,  Job <job> 
Then I should be displayed with response <StatusCode> for API "ReqresAPIPUTUpdateSingleUser"
And response should contain Name <name>,  Job <job> 
Examples: 
| page | StatusCode | name     | job           |
| 2    | 200        | morpheus | zion resident |


Scenario Outline: PATCH event for single user 
Given I access the API endpoint "ReqresAPIPATCHUpdateSingleUser" with <page>
When I Generate PATCH API request for ReqresAPIPATCHUpdateSingleUser with Name <name>,  Job <job> 
Then I should be displayed with response <StatusCode> for API "ReqresAPIPATCHUpdateSingleUser"
And response should contain Name <name>,  Job <job> 

Examples:
| page | StatusCode | name     | job           |
| 2    | 200        |  morpheus | zion resident |