cmake_minimum_required(VERSION 3.19.1)
project(unity-examples)

set(UNITY_PATH "/Applications/Unity/Unity.app/Contents/MacOS")
set(PROJECT_PATH "${CMAKE_BINARY_DIR}")

file(GLOB_RECURSE ASSET_FILES "${CMAKE_SOURCE_DIR}/Assets/*")

# Add a custom target to trigger copying of files
add_custom_target(copy_assets ALL
    COMMAND ${CMAKE_COMMAND} -E copy_directory ${CMAKE_SOURCE_DIR}/Assets ${CMAKE_BINARY_DIR}/Assets
    DEPENDS ${ASSET_FILES}
)

# Set the output directory for copied assets
set_property(TARGET copy_assets PROPERTY FOLDER "CMakePredefinedTargets")
set_target_properties(copy_assets PROPERTIES
    OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/Assets"
)

file(MAKE_DIRECTORY ${CMAKE_BINARY_DIR}/Builds/WebGL)

add_custom_target(webgl
    COMMAND ${UNITY_PATH}/Unity
            -quit
            -batchmode
            -nographics
            -projectPath ${PROJECT_PATH}
            -executeMethod WebGLBuilder.BuildWebGL
            -buildTarget WebGL
            -outputPath "${CMAKE_BINARY_DIR}/Builds/WebGL"
    DEPENDS copy_assets
)

set_property(DIRECTORY ${CMAKE_BINARY_DIR} PROPERTY VS_STARTUP_PROJECT ${PROJECT_NAME})
