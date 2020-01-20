#version 330 core

layout(location = 0) in vec2 aPosition;
layout(location = 1) in float aIndex;
layout(location = 2) in vec2 aTexCoord;

out vec3 texCoord;
flat out int texArray;
out vec4 texColorMod;

uniform int sheetW[16];
uniform int sheetH[16];

uniform float spriteX[64];
uniform float spriteY[64];
uniform int spriteW[64];
uniform int spriteH[64];
uniform int spriteTX[64];
uniform int spriteTY[64];
uniform int spriteTI[64];
// Type: 0: sprite, 1: level, 2: background, 3: , 4: quad
uniform int drawType;
uniform vec2 spriteScale[64];
uniform float spriteRotate[64];
uniform vec4 colorModulation[64];
uniform mat4 projection;
uniform vec2 origin;


void main(void)
{
	int instanceID = gl_InstanceID;

	if (drawType == 2)
	{
		instanceID = 0;
	}

	if (drawType == 4)
	{
		int color = spriteTI[instanceID];
		int alpha = (color >> 24) & 0x000000FF;
		int red = (color >> 16) & 0x000000FF;
		int green = (color >> 8) & 0x000000FF;
		int blue = (color >> 0) & 0x000000FF; 
		texCoord = vec3(red / 255.0, green / 255.0, blue / 255.0);
		texArray = -alpha;
	}
	else if (spriteTI[instanceID] != -1)
	{
		float left = (spriteTX[instanceID]);
		float texW = (aTexCoord[0] * spriteW[instanceID]);
		float texelX = left + texW;
		float top = (spriteTY[instanceID]);
		float texH = (aTexCoord[1] * spriteH[instanceID]);
		float texelY = top + texH;

		int sheetWidth = sheetW[spriteTI[instanceID]];
		int sheetHeight = sheetH[spriteTI[instanceID]];
		texCoord = vec3(texelX / sheetWidth, texelY / sheetHeight, spriteTI[instanceID]);
		texArray = spriteTI[instanceID];
		texColorMod = colorModulation[instanceID];
	}
	else
	{
		texCoord = vec3(1, 0, 0);
		texArray = -255;
	}

	mat4 view = mat4(1.0);
	mat4 translatePos = mat4(1.0);
	mat4 scaleSize = mat4(1.0);
	mat4 scaleTransform = mat4(1.0);
	mat4 rotateTransform = mat4(1.0);

	view[0][3] = -1;
	view[1][3] = 1;

	if (drawType == 4)
	{
		translatePos[0][3] = -origin[0];
		translatePos[1][3] = -origin[1];

		vec2 vertexPosition = vec2(0.0);
		switch(int(aIndex))
		{
			case 0:
				vertexPosition = vec2(spriteX[instanceID], spriteY[instanceID]);
				break;
			case 1:
				vertexPosition = vec2(spriteW[instanceID], spriteH[instanceID]);
				break;
			case 2:
				vertexPosition = vec2(spriteTX[instanceID], spriteTY[instanceID]);
				break;
			case 3:
				vertexPosition = spriteScale[instanceID];
				break;
		}
		gl_Position = vec4(vertexPosition, 0.0, 1.0) * translatePos * projection * view;
	}
	else
	{
		if (drawType != 2)
		{
			translatePos[0][3] = spriteX[instanceID] - origin[0];
			translatePos[1][3] = spriteY[instanceID] - origin[1];
		}
		else
		{
			translatePos[0][3] = spriteX[instanceID] + spriteW[0] * (gl_InstanceID  % spriteW[1]);
			translatePos[1][3] = spriteY[instanceID] + spriteH[0] * int(gl_InstanceID / spriteW[1]);
		}

		scaleSize[0][0] = spriteW[instanceID];
		scaleSize[1][1] = spriteH[instanceID];
	
		scaleTransform[0][0] = spriteScale[instanceID][0];
		scaleTransform[1][1] = spriteScale[instanceID][1];
		float rotCos = cos(spriteRotate[instanceID]);
		float rotSin = sin(spriteRotate[instanceID]);
		rotateTransform[0][0] = rotCos;
		rotateTransform[0][1] = rotSin;
		rotateTransform[1][0] = -rotSin;
		rotateTransform[1][1] = rotCos;
		rotateTransform = rotateTransform;
		gl_Position = vec4(aPosition, 0.0, 1.0) * scaleTransform * scaleSize * rotateTransform * translatePos * projection * view;
	}
}