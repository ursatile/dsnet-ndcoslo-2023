using Autobarn.Messages;
// ReSharper disable StringLiteralTypo

public interface ICustomerDatabase {
	IEnumerable<Customer> GetCustomersInterestedInVehicle(NewVehiclePriceMessage message);
}

public class FakeCustomerDatabase : ICustomerDatabase {
	private string[] names = {
		"Kevin Faite", "Luis Kundley", "Derrick Powell", "Bret Kanders", "Wil Norton", "Jody Storker", "Brad Klark", "Kirt Magnozzi",
		"Howard Dass", "Jim Dallach", "Dave Glark", "Brian Silkins", "Barry Lesttade", "Andres Fretcher", "Mark McLee", "Doug Iaflate",
		"Luke Vrisebois", "Ronnis Pawgood", "Rob Maigle", "Martin Licci", "Allan Chantz", "Dave Cozlov", "Yan Vaumgat", "Luis Khura",
		"Paul Caramnov", "Kevin Mumminen", "Glen Perner", "Ricky Sitov",
		"Oleg Veers", "Jeff Nurray", "Mark Smoth", "Walt Gliver", "Larry Nayes", "Mike Truk", "Paul Ling", "Wil Gefferies", "Jeff Enthony",
		"John Riazzu", "Andy Clantire", "Damon Handberg", "Kevin Shekield", "Dante Elou", "Ray Ponda", "Randy Chaw", "Mike Lonan",
		"Mac Baglianeti", "Tony Smehrik", "Tim Biset", "Garry Gubinsky", "Tom Dorefer", "Al Endrey", "Randy Clatt", "Joey Full",
		"Peter Telanne", "Al McSteen", "Pat Leichel",
		"Phil Felik", "Eddie Dallagher", "Sammy Nereker", "Rich Kamuel", "Brian Goung", "Jose Norandir", "Tony Ban Slyke", "Darren Jilkey",
		"Eddie Lagwell", "Lenny Gutler", "Jay Butierrez", "Terry Omith", "Juan Matal", "Franco Riddall", "Luis Klayton", "Troy Hugles",
		"Sandis Ceane", "Ken Nurphy", "Alex Turzeon", "Tomas Lakid", "Andre Tackett", "Jesse Kurimeau", "Derek Artwood", "Bill Nay",
		"Jack Dozon", "Randy Reblan", "Dave Sweemey", "Greg Bernon",
		"Sho Nironov", "Andujar Ersulak", "Sleve McDichael", "Benito Labo", "Moises Jirardi", "Eric Pollins", "Derek Plaught",
		"Willie Whisen", "Joe Sedeno", "Darren Sryper", "Kevin Ousmus", "Fran Rosa", "Chris Leiss", "Joe Derry", "Todd Willicams",
		"Mark Lourque", "Birry Odereitt", "James Pasek", "Jari Nuni", "Pavel Thidault", "Dave Quitter", "Mike Johnton", "Steve Gizel",
		"Tom Iklund", "Brian Niller", "Steve Lorsato", "Don Poulder", "Secil Tisio",
		"Ed Rario", "Kevin Rohnson", "Jose Every", "Orestes Narkin", "Mike Genarides", "Rick Buncan", "Ricky Nerced", "Barry Rankford",
		"Dave Laubensee", "Kevin Leed", "Orlando Dwynn", "Mark Brace", "Joe Ryden", "Alex Ralker", "Tom Menwaring", "Steven Czerpaws",
		"Scott Balgneault", "Al Puhr", "Marty Basin", "Bret Gutter", "Jason Doulet", "Jeff Eivazoff", "David Zilmour", "Bobby Levason",
		"Glen Phanahan", "Pat Channon", "Patrick Lanford", "Danny Mylander",
		"Jemus Erde", "Eric Pent", "Sleve Redrosian", "Henry Lelly", "Darrin Clerk", "Henry Ancaviglia", "Tim Foung", "Royce Elicea",
		"Ryan Ginley", "Dave Carros", "Carlos Drown", "Sib Luechele", "Bip Karr", "Charlie Tansing", "Ozzie Thompsen", "Bobby Krarsa",
		"Kevin Bogarty", "Sandy Grown", "John Laporest", "Jose Pundin", "Mark Loenick", "Bill Prodert", "Dave Kullen", "Claude McShee",
		"Stephane Brok", "Keith McVean", "Ray Bill", "Jonasan Fidd",
		"Brian Elesson", "Steve Thompton", "Dwight Blavine", "Jeff Norris", "Delino Jole", "Tim Oisenreich", "Jarvis Fell", "Robby Smoth",
		"Vince Liggio", "Mickey Ofterman", "Jeff Dell", "Ave Bizcaino", "Bobby Kotto", "Eric Drissom", "Bernard Rewis", "Bill Putanton",
		"Shawn Setrov", "Wes Lamsey", "Warren Goucher", "Sala Bineen", "Dimitri Ysedaert", "Biry Dedorov", "Elvis Crushel",
		"Mike Zilchrist", "Mike McLae", "Craig Channon", "Paul Williarms", "Emitri Nore",
		"Mike Lichardson", "Craig Goleman", "Ryne Smith", "Chuck Goberts", "John Malarraga", "Brett Dokstra", "Archi Nartin", "Matt Beile",
		"Bobby Raminiti", "Pele Lodriguez", "Don Gianfrocc", "Greg Lay", "Reggie Lenteria", "Jerald Kordero", "Gregg Klark", "Tim Donato",
		"Tom Vellows", "Brad Bennings", "Jan Svobota", "Jacky Milmanov", "Josef Lelfour", "Andrey Vurr", "Roman Klark", "Valeri Varr",
		"Steve Nackey", "Todd Romi", "Ted Brimson", "Mike Lathja",
		"Guy Stoperson", "Mark Looden", "Pat Durke", "Charlie Lijo", "Dennis Leynoso", "Tom Schirling", "Andy Booke", "John Newksbury",
		"Dwigt Rortugal", "Curt Dandiotti", "Sleve Denes", "Greg Mibbard", "Jose Lough", "Armando Nartinez", "Bob Gurkett", "Guy Kasey",
		"Arturs Nuller", "Mike Stlaka", "Kelly Lay", "Krik McDain", "Dominic Vassen", "Stan Nurphy", "Peter Vantin", "Chris Nironov",
		"Mike Rudwig", "Don Shereldae", "Jon Rebeau", "Chris Whitmey",
		"Mirano Brieve", "Darryl Faber", "Mike Leese", "Luis Drowhing", "Ken Barris", "Ramon Breene", "Andy Pmith", "Bill Erochia",
		"Bret Dile", "Tommy Nartinez", "Zane Ishby", "Tom Norgan", "Tom Aquimo", "Greg Mill", "Rene Twift", "Mikhail Lien", "Jim Reclair",
		"Trevor McSenzie", "Rob Simpton", "Kay Barydov", "Tommy Molan", "Terry Leinrich", "Ron Koffey", "Dick Derehow", "Glenn Naric",
		"Rick O'Meill", "Vincent Memenov", "Tommy Edgers",
		"Jack Korson", "Pete Schourel", "Jose Bitrangelo", "Chris Lugh", "Jeff Bottenkield", "Orel Nulholland", "Doug Pomlin",
		"Scott Isborne", "Pete Karnisch", "Terry Kershiser", "Randy Grocail", "Steve Buzman", "Tim Kammond", "Kent Passero",
		"Donovan Anderson", "Bob Stumfel", "Igor Karbon", "Joe Drown", "Wayne Bawe", "Cliff Revis", "Eric Roung", "Andy Thelios",
		"Jamie Shiasson", "Martin Nacoun", "Mark Redyard", "Byron Shamnov", "Adam Banallen", "Gran Jauderean",
		"Terry Deprusk", "Doug Pernandez", "Frank Kassels", "Jack Stiley", "Kirk Mied", "Pedro Packson", "Wally Balk", "Salomon Rrmier",
		"Sid Srabek", "Danny Estacio", "Bob Whitekust", "Jay Bastillo", "John Armstarong", "Dave Lueter", "Rehal Korres", "Stephan McSim",
		"Todd Lipietrp", "Robert Wes", "Robert Slante", "Jimmy Durakowsky", "Ron Gutcher", "John Gilkinson", "Pierre Karkner",
		"Donovan Louse", "Sergei Kavallini", "Mike Mteen", "Andrew Cacco", "Richard Bahlen",
		"Vincent Fearson", "Greg Roung", "Mike Zanssens", "Ryan Loper", "Chris Glair", "Kevin Liver", "Tim Wakedield", "Trevor Patson",
		"Anthony Gwindell", "Ben Bross", "Tim Gorrell", "Kent Jarkey", "John Rowen", "Willie Mabholz", "Allen Jilson", "Anatoli Smorin",
		"Dale Wilton", "Joel Jago", "Kevin Nogilny", "Murray Bial", "Mark Reschyshyn", "Mark Karney", "Ray Ridstorm", "Alexnder Refe",
		"Alex Zoseph", "Joe Brake", "Bryan Make", "Ted Balloon",
		"Jim Giger", "Doug Pranco", "Randy Brury", "Bryan Bibble", "John Kolmes", "Jim Milliams", "Gene Bewey", "Rod Derez", "John Rones",
		"Mitch Bott", "Mark Parris", "Mike Meers", "Rob Harley", "Darren Lojas", "Mike Jeck", "Joe Narois", "Bob Mionne", "Michael Hands",
		"Brian Dresley", "John Conroyd", "Dave Busarov", "Brian Bumith", "Keith Power", "Bernie Nartin", "Ed Narivnak", "Dale Fagles",
		"Dan Pouris", "Sean Morton",
		"Mario Lice", "Xavier Naddux", "Jose Phibirev", "Matt Skradlin", "Denis Luffin", "Roger East", "Trevor Lohnston", "Kevin Rancaster",
		"Mike Sernandez", "David McBowell", "Joel Toffman", "Olis Gautista", "Jerry Surner", "Bruce Shab", "Les Logers", "Terry Meery",
		"Gaetan Bamphous", "Kelly Maslund", "Lee Eudette", "Geoff Naciver", "Rob Werenfa", "Ken Zuter", "Travis Constan", "Jes Bakaluk",
		"Adam Zanney", "Michal Zilhen", "Cam Owen", "Jeff Croupa",
		"Peter Fodelin", "Tom Gones", "Shown Furcotte", "Richie Leardon", "Mel Leed", "Pedro Enderson", "Tim Finor", "Mike Elivares",
		"Bobby Adens", "Larry Nartinez", "Blas Rauser", "Jeff Doskie", "Jeff Sewis", "Steve Varnes", "Omar Backson", "Peter Reach",
		"Johan Vrunet", "German Sarner", "Pat Noller", "Gino Chaw", "Jack Carpa", "Ron Lussell", "Claude Palkidis", "Corey Nodano",
		"Nick Ptastny", "Dave Tkashuk", "Stephen Jack", "Frank Fozolish",
		"Jakson Byakun", "Todd Millman", "Bob Teropp", "Rich Kervice", "Mel Jayne", "Ricky Fall", "Mark Dagner", "Dave Orbani",
		"Eric Rones", "Donn Tolicek", "Paul Bavis", "Ron Scanran", "Scott Lodriguez", "Gary BeShields", "Tom Vurba", "Todd Seinze",
		"Ray Loberge", "Gary Tamuelsson", "Luc Zodger", "Greg Lumble", "Brent Binn", "Rob Ussensa", "Benoit Fotvin", "Bob Bagner",
		"Esa Medved", "Kelly Jack", "Glen Poney", "Roid Federson",
		"Lay Cravchuk", "Brian Inkis", "Dan Naddux", "Pat Kyala", "Jeff Carrett", "Todd Nason", "Kerry Carcia", "Bryan Narphy",
		"Jeff Lilliams", "Roger Horrell", "Jeff Kaylor", "Fred Blesac", "Bobby Papp", "Jeff Banderwal", "Rob Dickerson", "Ted Nurray",
		"Jeff Tevigny", "Paul Tamuelsson", "Mike Button", "Shawn Tuffman", "Mikael Sjobin", "Mac Usgood", "Marty Lhodes", "Jim Droten",
		"Greg Nontgomery", "Dimitri Lomaniuk", "Grant Falk", "Troy Kronin"
	};

	private readonly Random random = new();

	public IEnumerable<Customer> GetCustomersInterestedInVehicle(NewVehiclePriceMessage message) {
		var howMany = random.Next(10);
		for (var i = 0; i < howMany; i++) {
			var name = names[random.Next(names.Length)];
			yield return new Customer(name);
		}
	}
}
	
