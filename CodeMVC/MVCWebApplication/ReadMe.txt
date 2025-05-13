** This Project follows Onion Architechture
- Following Libraries are being used in this architecture.

	-> NaCore: Contains Domain Models and Abstraction/Interface of repositories.

	-> NaData: Contains Implementation of Entity Specific repositories which is 
				responsible for interaction with Database.

	-> NaServices: It is a layer which acts as a bridge between NaData Layer
				and Main UI Layer, Basically it hides the implementation of
				repositories from UI. UI Layer is not aware of how the data is
				fetched or stored in Database. Although the business logic should 
				be present in Service layer only.

	-> NaWebsite: This is a main UI layer responsible to facilitate requests from
				client/Front end. This layer communicates with NaServices layer to
				fetch or perform CRUD operations. Automapper is also being used here
				to Map request models to the Domain Models. This layer is also responsible
				for Authentication, Authorization and Filters being used in application.
