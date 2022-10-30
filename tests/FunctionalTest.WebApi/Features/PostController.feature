Feature: PostController

A short summary of the feature

@GetAll
Scenario: Get All Posts
	When Send HTTP GET request to URL "/api/v1.0/post"
	Then The response should not be empty
