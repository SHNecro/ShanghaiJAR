#version 330

out vec4 outputColor;
in vec3 texCoord;
flat in int texArray;
in vec4 texColorMod;

uniform sampler2D textureArrays[16];

void main()
{
	if (texArray < 0)
	{
		outputColor = vec4(texCoord, -texArray / 255.0);
	}
	else
	{
		vec4 rawColor;
		// OpenGL 3.30 spec 4.1.7 p.21 requires constant array index for array of samplers
		switch (texArray)
		{
			case 0: rawColor = texture(textureArrays[0], vec2(texCoord)); break;
			case 1: rawColor = texture(textureArrays[1], vec2(texCoord)); break;
			case 2: rawColor = texture(textureArrays[2], vec2(texCoord)); break;
			case 3: rawColor = texture(textureArrays[3], vec2(texCoord)); break;
			case 4: rawColor = texture(textureArrays[4], vec2(texCoord)); break;
			case 5: rawColor = texture(textureArrays[5], vec2(texCoord)); break;
			case 6: rawColor = texture(textureArrays[6], vec2(texCoord)); break;
			case 7: rawColor = texture(textureArrays[7], vec2(texCoord)); break;
			case 8: rawColor = texture(textureArrays[8], vec2(texCoord)); break;
			case 9: rawColor = texture(textureArrays[9], vec2(texCoord)); break;
			case 10: rawColor = texture(textureArrays[10], vec2(texCoord)); break;
			case 11: rawColor = texture(textureArrays[11], vec2(texCoord)); break;
			case 12: rawColor = texture(textureArrays[12], vec2(texCoord)); break;
			case 13: rawColor = texture(textureArrays[13], vec2(texCoord)); break;
			case 14: rawColor = texture(textureArrays[14], vec2(texCoord)); break;
			case 15: rawColor = texture(textureArrays[15], vec2(texCoord)); break;
		}

		outputColor = vec4(rawColor[0] * texColorMod[0], rawColor[1] * texColorMod[1], rawColor[2] * texColorMod[2], rawColor[3] * texColorMod[3]);
	}
}