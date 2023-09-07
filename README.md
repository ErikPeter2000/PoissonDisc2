# PoissonDisc2

This is a WPF application for quickly rendering and saving tiled voronoi textures as .png files for use in external applications.
Vornoi images are made by scattering some points (samples) on a grid and then coloring the individual pixels based on their distances to these points.

Currently, this applications supports voronoi:
- Distance to point: The classic, also called celular noise or worley noise. Pixel intensity is defined by the distance to the nearest sample.
- Distance to edge: Pixel intensity is defined by the distance to the perpendicular bisector given by the closest two points.
- Random color: Pixel intensity is randomly generated with the closest pixel as the seed.
- Raw pixel: If the pixel is a position of a sample, then make it white, else it is black. 

Voronoi textures are used a lot when doing 3D graphics. Blender, the graphics design software I use, uses the jittered distribution of points which looks trash. The poisson disc sampling method gives a much more natural appearance. This sadly meant I had to write my own algorithm for generating this texture.

Algorithm based on this paper [https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf]
