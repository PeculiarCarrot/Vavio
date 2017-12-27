initialized = {}
anglePer = {}
numBullets = 9

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	for i=0, numBullets-1, 1 do
			bullet = pattern.NewBullet()
			bullet.scale = 4
			bullet.type = "circle"
			bullet.material = "orange"
			bullet.speed = pattern.Math().RandomRange(1.75, 3.75)
			bullet.x = bullet.x + pattern.Math().RandomRange(-10, 10)
			pattern.SpawnBullet(bullet)
		end
end