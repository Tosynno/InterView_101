Feature: UserLogin
  As a user
  I want to receive a token
  So I can use the API that requires authentication

  Scenario: Successfully receive a token
    Given I have a customer in database
      | Name      | ServiceId       | Password        |
      | Customer1 | 436564777474    | ComplexPass123! |
    And I have a login data
      | ServiceId       | Password        |
      | 436564777474    | ComplexPass123! |
    When I call the Login API
    Then I should receive a success response
    And response has a valid token

  Scenario Outline: Return an error if invalid credentials have been provided
    Given I have a customer in database
      | Name      | ServiceId       | Password        |
      | Customer1 | 566564777474    | ComplexPass123! |
    And I have a login data
      | ServiceId       | Password        |
      | <ServiceId>     | <Password>      |
    When I call the Login API
    Then I will receive an error code 401
    And Response message should contain "Invalid ServiceId or password provided."
  
    Examples:
      | Scenario Description  | ServiceId       | Password        |
      | Wrong ServiceId       | 43656477474     | ComplexPass123! |
      | Wrong Password        | 436564777474    | ComplexPass     |

  Scenario Outline: Return an error if credentials are not provided
    Given I have a customer in database
      | Name      | ServiceId        | Password        |
      | Customer1 | 436564777474     | ComplexPass123! |
    And I have a login data
      | ServiceId       | Password        |
      | <ServiceId>     | <Password>      |
    When I call the Login API
    Then I will receive an error code 401
    And Response message should contain "<Error>"
  
    Examples:
      | Scenario Description  | ServiceId       | Password        | Error                  |
      | ServiceId not provided|                 | ComplexPass123! | ServiceId is required  |
      | Password not provided | 436564777474    |                 | Password is required   |