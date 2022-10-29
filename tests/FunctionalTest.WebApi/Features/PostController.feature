Feature: PostController

A short summary of the feature

@GetAll
Scenario: Get All Posts
	When GetPosts method is called
	Then The response should not be empty
