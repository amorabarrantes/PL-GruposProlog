
####### Expanded from @PACKAGE_INIT@ by configure_package_config_file() #######
####### Any changes to this file will be overwritten by the next CMake run ####
####### The input file was SWIPLConfig.cmake.in                            ########

get_filename_component(PACKAGE_PREFIX_DIR "${CMAKE_CURRENT_LIST_DIR}/../../../" ABSOLUTE)

####################################################################################

# Version information
set(SWIPL_VERSION_COUNT 3)
set(SWIPL_VERSION_MAJOR 8)
set(SWIPL_VERSION_MINOR 2)
set(SWIPL_VERSION_PATCH 2)
set(SWIPL_VERSION_STRING 8.2.2)

# Location suitable for passing to PL_initialise() as argv[0]
set(SWIPL_EXECUTABLE "${PACKAGE_PREFIX_DIR}/./bin/libswipl.dll")

# Exported targets
include("${CMAKE_CURRENT_LIST_DIR}/SWIPLTargets.cmake")
