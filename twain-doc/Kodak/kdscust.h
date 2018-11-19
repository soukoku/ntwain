/////////////////////////////////////////////////////////////////////////////
//
//  Kodak Document Scanner TWAIN Source
//  Custom Stuff...
//
//  Copyright (c) 1998-2013 Eastman Kodak Company, All Rights Reserved
//
//  Altering the information in this file is not authorized except
//  with the express consent of Eastman Kodak Company.  The values
//  of the constants in this file are set at the discretion of
//  Eastman Kodak Company.  New constants may be added at any
//  time and old ones may be removed or modified at any time.
//
//  The capabilities contained in this header file are unique to the
//  Kodak Digital Science(tm) Scanners.  As such, the application
//  has a responsibility to verify that it is communicating with
//  this Source before attempting to use them.  Verification is
//  accomplished by examining the TW_IDENTITY structure returned by
//  the Source from the DG_CONTROL / DAT_PARENT / MSG_OPENDS call.
//  This Source reports its identity in the following fields:
//
//    TW_IDENTITY
//        Manufacturer     "Eastman Kodak Company"
//        Version.Info     "KDS v#.#.# YYYY/MM/DD"
//
//  By looking at the Manufacturer name, and looking for KDS as the
//  first three letters in the Version.Info, it should be possible
//  for an application to unambiguously identify this Source.
//
//
//  The Family names break down into the following models...
//
//    GEMINI (kds):
//      500,  500A,
//      900,  923,  990,
//      5500, 5520,
//      7500, 7520, 7550, 7560,
//      9500, 9520
//
//    VIPER (kds):
//      3500, 3510, 3520, 3590,
//      4500
//
//    PRISM (kds_i800):
//      i810, i820, i830, i840
//
//    PHOENIX (kds_i600):
//      i610, i620, i640, i660
//
//    ALIEN (kds_i200):
//      i250, i260, i280
//
//    Alf (kds_i100):
//      i150, i160
//
//    Pony (kds_i30_i40):
//      i30, i40, i40T
//
//    MUSTANG2 (kds_i55_i65):
//      i55, i65
//
//    PIRANHA (kds_i1300):
//      i1310, i1320
//
//    PIRANHA1200 (kds_i1200):
//      i1210, i1220
//
//    WILDFIRE (kds_i1800):
//      i1840, i1860
//
//    A2O2 (kds_i1400):
//      i1410, i1420, i1440
//
//    Fosters (kds_i1100):
//      i1120
//
//    Inferno (kds_i700):
//      i720, i730, i750, i780
//
//    Panther (kds_i4200_i4600):
//      i4200, i4600
//
//    Blaze (kds_i5000):
//      i5200, i5600, i5800
//
//    Piranha2 (kds_i2000):
//      i2400, i2600, i2800
//
//    Rufous (kds_i900):
//      i920
//
//    FalconA4 (kds_i2900):
//      i2900
//
//    FalconA3 (kds_i3000):
//      i3000
//
//    Piranha2stw (kds_pss): [Note: This scanner family supports the same
//      PS50, PS80                  list of capabilities as the Piranha2
//                                  (kds_i2000) scanner family.]
//
//  For more information about these capabilities and the driver,
//  please refer to the Integrator's Guide on the media that came
//  with your scanner.
//
//  **************************************************************
//  This file does not conform to the 2-byte packing rule for
//  TWAIN, it should use the default packing for the compiler...
//  **************************************************************
//  
/////////////////////////////////////////////////////////////////////////////

#ifndef KDSCUST_H
#define KDSCUST_H



////////////////////////////////////////////////////////////////////////////////
//                               INCLUDE FILES
////////////////////////////////////////////////////////////////////////////////

#ifdef _WIN32
#include "twain.h"
#endif



////////////////////////////////////////////////////////////////////////////////
//                      DEFINES, TYPEDEFS, CONSTS & ENUMS
////////////////////////////////////////////////////////////////////////////////



////////////////////////////////////////////////////////////////////////////////
//                      CAP Section
////////////////////////////////////////////////////////////////////////////////



// CAP_BACKGROUND
// Family:      A2O2, Alf, Blaze, Falcon, Fosters, Inferno, Mustang2, Panther,  
//              Phoenix, Piranha, Piranha2, Rufous, Wildfire
// Type:        TWTY_INT16
// Container:   Enumeration
// Allowed:     TWBK_BLACK, TWBK_WHITE
// Default:     (scanner dependent)
// Notes:       Reports what the scanner background was at the
//              time the scanner was started.  This capability
//              cannot detect a "hot" change.
//              For Blaze and Panther, it allows the user to select the color
//              of the imaging background. This can be set differently per side.
#define CAP_BACKGROUND                      0x8089
#define   TWBK_BLACK                        0
#define   TWBK_WHITE                        1


// CAP_BACKGROUNDFRONT
// Family:      A2O2, Alf, Blaze, Falcon, Fosters, Panther, Piranha, Piranha2,
//              Rufous
// Type:        TWTY_INT16
// Container:   Enumeration
// Allowed:     TWBF_BLACK, TWBF_WHITE
// Default:     (scanner dependent)
// Notes:       Reports what the scanner front background was at the
//              time the scanner was started.  This capability
//              cannot detect a "hot" change.
#define CAP_BACKGROUNDFRONT                 0x808C
#define   TWBF_BLACK                        0
#define   TWBF_WHITE                        1


// CAP_BACKGROUNDREAR
// Family:      A2O2, Alf, Blaze, Falcon, Fosters, Panther, Piranha, Piranha2,
//              Rufous
// Type:        TWTY_INT16
// Container:   Enumeration
// Allowed:     TWBR_BLACK, TWBR_WHITE
// Default:     (scanner dependent)
// Notes:       Reports what the scanner rear background was at the
//              time the scanner was started.  This capability
//              cannot detect a "hot" change.
#define CAP_BACKGROUNDREAR                  0x808D
#define   TWBR_BLACK                        0
#define   TWBR_WHITE                        1


// CAP_BACKGROUNDPLATEN
// Family:      n/a
// Type:        TWTY_INT16
// Container:   Enumeration
// Allowed:     TWBP_BLACK, TWBP_WHITE
// Default:     (scanner dependent)
// Notes:       Reports what the scanner background was at the
//              time the scanner was started.  This capability
//              cannot detect a "hot" change.
//              This capability is not available at this time.
#define CAP_BACKGROUNDPLATEN                0x808E
#define   TWBP_BLACK                        0
#define   TWBP_WHITE                        1


// CAP_BATCHCOUNT
// Family:      Prism, Wildfire
// Type:        TWTY_FIX32
// Container:   Range
// Allowed:     0 - 32767
// Default:     0
// Notes:       Count of CAP_BATCHLEVEL's...
#define CAP_BATCHCOUNT                      0x802B


// CAP_BATCHENDFUNCTION
// Family:      Prism, Wildfire
// Type:        TWTY_INT16
// Container:   Enumeration (per camera)
// Allowed:     TWPL_NONE, TWPL_LEVEL1, TWPL_LEVEL2, TWPL_LEVEL3
// Default:     TWPL_NONE
// Notes:       Determines the level that batching will count
//              and test against the batch counting value.
#define CAP_BATCHENDFUNCTION                0x804F
#define   TWBE_NONE                         0
#define   TWBE_STOPFEEDER                   1
#define   TWBE_ENDOFJOB                     2
#define   TWBE_NEWBATCH                     3


// CAP_BATCHLEVEL
// Family:      Prism, Wildfire
// Type:        TWTY_INT16
// Container:   Enumeration (per camera)
// Allowed:     TWPL_LEVEL1, TWPL_LEVEL2, TWPL_LEVEL3
// Default:     TWPL_LEVEL1
// Notes:       Determines the level that batching will count
//              and test against the batch counting value.
#define CAP_BATCHLEVEL                      0x804E


// CAP_BATCHSTARTFUNCTION
// Family:      Prism, Wildfire
// Type:        TWTY_INT16
// Container:   Enumeration (per camera)
// Allowed:     TWPL_NONE, TWPL_LEVEL1, TWPL_LEVEL2, TWPL_LEVEL3
// Default:     TWPL_NONE
// Notes:       Level that a batch should start in...
#define CAP_BATCHSTARTFUNCTION              0x803F


// CAP_BINARIZATION
// Family:      Viper (3590 only)
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Controls the presence of binarization in the
//              camera control.  It also overrides the value
//              of the front bitonal CAP_CAMERAENABLE.
#define CAP_BINARIZATION                    0x8030


// CAP_BLANKPAGE
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Mustang2, Panther, Phoenix,
//              Piranha, Piranha2, Pony, Rufous, Wildfire
// Type:        TW_UINT16
// Container:   Enumeration
// Allowed:     TWBP_IMAGE
// Default:     TWBP_IMAGE
// Notes:       Blank image deletion (values 0 and 2 can not used, they became obsolete)
#define CAP_BLANKPAGE                   0x809A
#define   TWBP_IMAGE                    1


// CAP_BLANKPAGEMODE
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Mustang2, Panther, Phoenix,
//              Piranha, Piranha2, Pony, Rufous, Wildfire
// Type:        TW_UINT16
// Container:   Enumeration
// Allowed:     TWBM_NONE, TWBM_COMPSIZE, TWBM_CONTENT
// Default:     TWBM_NONE
// Notes:       Blank image deletion mode. Not all values are supported on all scanners.
#define CAP_BLANKPAGEMODE               0x809B
#define   TWBM_COMPSIZE                 0
#define   TWBM_NONE                     1
#define   TWBM_CONTENT                  2


// CAP_BLANKPAGECOMPSIZEBW
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Mustang2, Panther, Phoenix,
//              Piranha, Piranha2, Pony, Rufous, Wildfire
// Type:        TW_UINT32
// Container:   Range
// Allowed:     0 to 1000KB
// Default:     0
// Notes:       Delete Bitonal image if the final size is less than specific amount.
//              Value needs to be in 1024 increments
//              The front and rear values must be the same on Mustang2 and Pony
//              When set to a non-zero value: CAP_BLANKPAGEMODE is automatically
//              set to TWBM_COMPSIZE
//              If set zero and CAP_BLANKPAGECOMPSIZEBW and CAP_BLANKPAGECOMPSIZEGRAY
//              are zero, then CAP_BLANKPAGEMODE is automatically changed to TWBM_NONE
#define CAP_BLANKPAGECOMPSIZEBW          0x809C


// CAP_BLANKPAGECOMPSIZEGRAY
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Mustang2, Panther, Phoenix,
//              Piranha, Piranha2, Pony, Rufous, Wildfire
// Type:        TW_UINT32
// Container:   Range
// Allowed:     0 to 1000KB
// Default:     0
// Notes:       Delete Gray image if the final size is less than specific amount.
//              Value needs to be in 1024 increments
//              The front and rear values must be the same on Mustang2 and Pony
//              When set to a non-zero value: CAP_BLANKPAGEMODE is automatically
//              set to TWBM_COMPSIZE
//              If set zero and CAP_BLANKPAGECOMPSIZEBW and CAP_BLANKPAGECOMPSIZEGRAY
//              are zero, then CAP_BLANKPAGEMODE is automatically changed to TWBM_NONE
#define CAP_BLANKPAGECOMPSIZEGRAY          0x809D


// CAP_BLANKPAGECOMPSIZERGB
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Mustang2, Panther, Phoenix,
//              Piranha, Piranha2, Pony, Rufous, Wildfire
// Type:        TW_UINT32
// Container:   Range
// Allowed:     0 to 1000KB
// Default:     0
// Notes:       Delete Color image if the final size is less than specific amount.
//              Value needs to be in 1024 increments
//              The front and rear values must be the same on Mustang2 and Pony
//              When set to a non-zero value: CAP_BLANKPAGEMODE is automatically
//              set to TWBM_COMPSIZE
//              If set zero and CAP_BLANKPAGECOMPSIZEBW and CAP_BLANKPAGECOMPSIZEGRAY
//              are zero, then CAP_BLANKPAGEMODE is automatically changed to TWBM_NONE
#define CAP_BLANKPAGECOMPSIZERGB           0x809E


// CAP_BLANKPAGECONTENT
// Family:      A2O2, Blaze, Falcon, Panther, Piranha, Piranha2
// Type:        TW_UINT32
// Container:   Range
// Allowed:     0 to 100
// Default:     0
// Notes:       If the percent of content on the image is less than or equal to
//              this amount, the image will be deleted. This is only valid when
//              CAP_BLANKPAGEMODE is set to TWBM_CONTENT
#define CAP_BLANKPAGECONTENT		     0x80C4


// CAP_CAMERAENABLE
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, Mustang2, 
//              Panther, Phoenix, Piranha, Piranha2, Pony, Prism, 
//              Rufous, Wildfire, Viper
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     TRUE (except for the 3590 front bitonal)
// Notes:       Controls the delivery of images.  If this capability
//              is set to TRUE then the Source will deliver an image
//              for this camera during scanning.  You need to use
//              DAT_FILESYSTEM to address each of the cameras.
#define CAP_CAMERAENABLE                    0x801D


// CAP_CAMERAORDER
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, 
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, 
//              Prism, Rufous, Wildfire, Viper
// Type:        TWTY_UINT16
// Container:   Array
// Allowed:     TWCM_BW_BOTH, TWCM_CL_BOTH
// Default:     TWCM_CL_BOTH, TWCM_BW_BOTH
// Notes:       Selects the ordering of the cameras according to the
//              order that they appear in the list.  So the default
//              indicates that a color front or rear will precede a
//              bitonal front or rear.
//
// There is a conflict between the standard TWAIN CAP_CAMERAORDER and
// the Kodak custom CAP_CAMERAORDER.  The standard version has a value
// of 0x1037 and uses the TWPT_* values.  The Kodak version can be seen
// below.  This driver does not support the TWAIN 2.0 version of this
// capability.  An application must use the Kodak version to talk to a
// Kodak driver.
#ifdef CAP_CAMERAORDER
	#undef  CAP_CAMERAORDER
#endif
#define CAP_CAMERAORDERSTANDARD				0x1037
#define CAP_CAMERAORDER						0x801E
#define    TWCM_BW_BOTH                     0
#define    TWCM_BW_TOP                      1
#define    TWCM_BW_BOTTOM                   2
#define    TWCM_CLBW_BOTH                   3
#define    TWCM_CL_TOP                      4
#define    TWCM_CL_BOTTOM                   5
#define    TWCM_CL_BOTH                     6
#define    TWCM_GR_TOP                      7
#define    TWCM_GR_BOTTOM                   8
#define    TWCM_GR_BOTH                     9


// CAP_CHECKDIGIT
// Family:      Prism, Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       To set the check image address digit ON/OFF.
#define CAP_CHECKDIGIT                      0x808B


// CAP_DOCUMENTCOUNT
// Family:      Gemini
// Type:        TWTY_STR32
// Container:   OneValue
// Allowed:     0 - 999,999,999
// Default:     0
// Notes:       5000/7000/9000 only.  Sets the document count, but
//              only if CAP_PRINTERENABLED is either undefined or
//              set to FALSE.
#define CAP_DOCUMENTCOUNT                   0x8017


// CAP_DOCUMENTCOUNTENABLED
// Family:      Gemini
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       5000/7000/9000 only.  Determines whether or not
//              the CAP_DOCUMENTCOUNT is to be downloaded.
#define CAP_DOCUMENTCOUNTENABLED            0x8018


// CAP_DOUBLEFEEDENDJOB
// Family:      Inferno, Blaze, Falcon, Panther, Phoenix, Piranha2, Prism,
//              Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Controls the action the scanner takes when a double
//              feed is detected.  A value of TRUE will cause the
//              session to be terminated.  A value of FALSE will
//              cause the multifeed to be ignored (though an audible
//              alarm will still be sounded, if it is turned on).
#define CAP_DOUBLEFEEDENDJOB                0x806E


// CAP_DOUBLEFEEDSTOP
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, 
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, Prism, 
//              Rufous, Wildfire, Viper
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Controls the action the scanner takes when a double
//              feed is detected.  A value of TRUE will cause the
//              session to be terminated.  A value of FALSE will
//              cause the multifeed to be ignored (though an audible
//              alarm will still be sounded, if it is turned on).
#define CAP_DOUBLEFEEDSTOP                  0x8056


// CAP_DUALSTACKINGENABLED
// Family:      Blaze
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Indicates if the Controlled Dual Stacking Accessory is
//              enabled. It can only be enabled if the accessory is
//              installed.
#define CAP_DUALSTACKINGENABLED             0x8110


// CAP_DUALSTACKINGLENGTHMODE
// Family:      Blaze
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     Scanner specific
// Default:     TWDSLM_NONE
// Notes:       Indicates the length method being used to make document sorting decisions
//              for the Controlled Dual Stacking Accessory. The stacking length mode
//              only has meaning if CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGLENGTHMODE         0x8111
#define      TWDSLM_NONE                    0
#define      TWDSLM_LESSTHAN                1
#define      TWDSLM_GREATERTHAN             2
#define      TWDSLM_BETWEEN                 3


// CAP_DUALSTACKINGLENGTH1
// Family:      Blaze
// Type:        TWTY_FIX32
// Container:   Range
// Allowed:     Scanner-specific
// Default:     Scanner-specific (min range value)
// Notes:       Dual stacking length1 (in ICAP_UNITS).
//              If CAP_DUALSTACKINGLENGTHMODE is "less than", then any documents shorter
//              than this value will be placed in the selected  CAP_DUALSTACKINGSTACK.
//              If CAP_DUALSTACKINGLENGTHMODE is "greater than", then any documents longer
//              than this value will be placed in the selected CAP_DUALSTACKINGSTACK.
//              If CAP_DUALSTACKINGLENGTHMODE is "between", then any documents longer than
//              this value and shorter than CAP_DUALSTACKINGLENGTH2 will be placed in the
//              selected CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE and CAP_DUALSTACKINGLENGTHMODE
//              is set to any value other than TWDSLM_NONE.
//              The range is determined by the scanner, so an application
//              might want to ask what the range is.
#define CAP_DUALSTACKINGLENGTH1             0x8112


// CAP_DUALSTACKINGLENGTH2
// Family:      Blaze
// Type:        TWTY_FIX32
// Container:   Range
// Allowed:     Scanner-specific
// Default:     Scanner-specific (min range value)
// Notes:       Dual stacking length2 (in ICAP_UNITS).
//              If CAP_DUALSTACKINGLENGTHMODE is "between", then any documents longer than
//              CAP_DUALSTACKINGLENGTH1 and shorter than CAP_DUALSTACKINGLENGTH2 will be
//              placed in the selected CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE and CAP_DUALSTACKINGLENGTHMODE
//              is set to TWDSLM_BETWEEN.
//              The range is determined by the scanner, so an application
//              might want to ask what the range is.
#define CAP_DUALSTACKINGLENGTH2             0x8113


// CAP_DUALSTACKINGMULTIFEED
// Family:      Blaze
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Indicates if Multifeed documents should be placed in
//              the selected  CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGMULTIFEED           0x8114


// CAP_DUALSTACKINGPATCHTRANSFER
// Family:      Blaze
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Indicates if Patch Type Transfer documents should be placed in
//              the selected  CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGPATCHTRANSFER       0x8115


// CAP_DUALSTACKINGPATCHTYPE1
// Family:      Blaze
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Indicates if Patch Type 1 documents should be placed in
//              the selected  CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGPATCHTYPE1          0x8116


// CAP_DUALSTACKINGPATCHTYPE2
// Family:      Blaze
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Indicates if Patch Type 2 documents should be placed in
//              the selected  CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGPATCHTYPE2          0x8117


// CAP_DUALSTACKINGPATCHTYPE3
// Family:      Blaze
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Indicates if Patch Type 3 documents should be placed in
//              the selected  CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGPATCHTYPE3          0x8118


// CAP_DUALSTACKINGPATCHTYPE4
// Family:      Blaze
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Indicates if Patch Type 4 documents should be placed in
//              the selected  CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGPATCHTYPE4          0x8119


// CAP_DUALSTACKINGPATCHTYPE6
// Family:      Blaze
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Indicates if Patch Type 6 documents should be placed in
//              the selected  CAP_DUALSTACKINGSTACK.
//              Only valid if CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGPATCHTYPE6          0x811A
 
 
// CAP_DUALSTACKINGSTACK
// Family:      Blaze
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     Scanner specific
// Default:     TWDSS_STACK1
// Notes:       Which stack a document should drop into
//              based on stacking criteria. Only valid if 
//              CAP_DUALSTACKINGENABLED is TRUE.
#define CAP_DUALSTACKINGSTACK               0x811B
#define      TWDSS_STACK1                    1
#define      TWDSS_STACK2                    2


// CAP_EASYSTACKING
// Family:      Inferno, Phoenix, Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       If set to TRUE then the scanner makes adjustments
//              to improve the arrangement of the output stack as
//              the paper exits the transport.
#define CAP_EASYSTACKING                    0x8075


// CAP_ENABLECOLORPATCHCODE
// Family:      Viper (3590)
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Controls recognition of the 3590 patch page.
#define CAP_ENABLECOLORPATCHCODE            0x8054


// CAP_ENERGYSTAR
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, Mustang2, 
//              Panther, Phoenix, Piranha, Piranha2, Pony, Rufous, Wildfire
// Type:        TWTY_INT32
// Container:   Range
// Allowed:     Alien: 0, 15 - 60 (minutes)
//              Alf/Inferno/Mustang2/Phoenix/Pony/Wildfire: 0,  5 - 60 (minutes)
//              A2O2/Fosters/Piranha: 0, 5 - 240 (minutes)
//              Blaze/Panther/Piranha2: 5 - 240 (minutes)
//              Falcon/Rufous: 1 - 240 (minutes)
// Default:     15
// Notes:       Minutes of idle time before Energy Star kicks in...
#define CAP_ENERGYSTAR                      0x802F


// CAP_FEEDERKEEPALIVE
// Family:      Viper
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Controls the behavior of the feeder in the following way:
// 
// State       ICAP_XFERCOUNT       First Page       All Other Pages
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// FALSE             -1             Timeout          Timeout
// FALSE             >0             Timeout          Timeout
// TRUE              -1             Keep Alive       Timeout
// TRUE              >0             Keep Alive       Timeout
//
// The effect when TRUE from the user's perspective is that if the
// transport times out while waiting for the first sheet of paper,
// the Source will reenable the scanner, and start the transport
// back up again.  It will continue to do this until it gets the
// first sheet of paper, or until the user stops the scanner from
// the application.
#define CAP_FEEDERKEEPALIVE                 0x8001


// CAP_FEEDERMODE
// Family:      A2O2 (i1440), Alien (i280), Blaze, Falcon, Panther, Piranha2
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     A2O2, Alien: TWFM_NONE, TWFM_SPECIAL
//              Others: Scanner specific
// Default:     A2O2, Alien: TWFM_NONE
//              Others: Scanner specific
// Notes:       Selects TWFM_NONE will show Off on the UI
//              and TWFM_SPECIAL is On on the UI
//              CAP_FEEDERMODE is for Special Document
#define CAP_FEEDERMODE                      0x806F
#define      TWFM_NONE                      0
#define      TWFM_SPECIAL                   1
#define      TWFM_STACKINGIMPROVED          2
#define      TWFM_STACKINGBEST              3
#define      TWFM_FRAGILE                   4
#define      TWFM_LIGHTWEIGHT               5
#define      TWFM_THICK                     6
#define      TWFM_THIN                      7


// CAP_FIXEDDOCUMENTSIZE
// Family:      Viper (not 3500)
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Controls the scanner speed-up feature.  Note that this
//              feature cannot be used with overscan.  And it does
//              require that all the documents in the batch be of the
//              same size.
#define CAP_FIXEDDOCUMENTSIZE               0x8055


// CAP_FOLDEDCORNER
// Family:      Inferno, Phoenix
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     TWFC_DISABLED, TWFC_STOP, TWFC_ENDOFJOB
// Default:     TWFC_DISABLED
// Notes:       Controls Folded Corner detection and the action taken
//              if one is discovered during scanning...
#define CAP_FOLDEDCORNER                    0x8070
#define      TWFC_DISABLED                       0
#define      TWFC_STOPFEEDER                     1
#define      TWFC_ENDOFJOB                       2
#define TWCC_FOLDEDCORNER                   0x8001
#define TWDE_FOLDEDCORNER                   0x8001


// CAP_FOLDEDCORNERSENSITIVITY
// Family:      Inferno, Phoenix
// Type:        TWTY_UINT16
// Container:   Range
// Allowed:     1 - 100
// Default:     2
// Notes:       Controls the folded corner sensitivity, if folder
//              corner detection is turned on...
#define CAP_FOLDEDCORNERSENSITIVITY         0x8071


// CAP_FUNCTIONKEY*
// Family:      Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     TWFK_NONE
// Notes:       i800 series only.  Controls the action taken by the
//              function keys during a scanning session...
#define CAP_FUNCTIONKEY1                    0x8037
#define CAP_FUNCTIONKEY2                    0x8038
#define CAP_FUNCTIONKEY3                    0x8039
#define CAP_FUNCTIONKEY4                    0x803A // not used
#define      TWFK_NONE                         0
#define      TWFK_ENDOFJOB                     1
#define      TWFK_TERMINATEBATCH               2
#define      TWFK_SKIPMULTIFEED                3
#define      TWFK_SKIPPRINTING                 4
#define      TWFK_SKIPPATCH                    5
#define      TWFK_LOWERELEVATOR                6


// CAP_IMAGEADDRESS
// Family:      Gemini, Prism, Wildfire
// Type:        TWTY_STR255
// Container:   OneValue
// Allowed:     The image address string...
// Default:     Scanner Next Image Address (read from device)
// Notes:       800/5000/7000/9000 only.  Sets the image address.
#define CAP_IMAGEADDRESS                    0x8015


// CAP_IMAGEADDRESSENABLED
// Family:      Gemini
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       5000/7000/9000 only.  Determines whether or not
//              the CAP_IMAGEADDRESS is to be downloaded.
#define CAP_IMAGEADDRESSENABLED             0x8016


// CAP_IMAGEADDRESSTEMPLATES
// Family:      Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see below)
// Default:     TWIA_1
// Notes:       800 only.  Determines what kind of image address
//              format is allowed.  See CONST.INI for the user
//              selectable list of formats, and the language files
//              L_XX_XXX.INI for the associated text in the image
//              address presets dropdown list.  Note that, like
//              with ICAP_HALFTONES and ICAP_SUPPORTEDSIZES, it is
//              not really possible for an application to know
//              what the user is selecting, it just has to be
//              accepted blindly.
#define CAP_IMAGEADDRESSTEMPLATES           0x803E
#define      TWIA_CUSTOM                       -1
#define      TWIA_NONE                         0
#define      TWIA_1                            1
#define      TWIA_2                            2
#define      TWIA_3                            3
#define      TWIA_4                            4
#define      TWIA_5                            5
#define      TWIA_6                            6
#define      TWIA_7                            7
#define      TWIA_8                            8
#define      TWIA_9                            9
#define      TWIA_10                           10
#define      TWIA_11                           11
#define      TWIA_12                           12
#define      TWIA_13                           13
#define      TWIA_14                           14
#define      TWIA_15                           15
#define      TWIA_16                           16


// CAP_IMAGEADDRESS_A
// CAP_IMAGEADDRESS_B
// CAP_IMAGEADDRESS_C
// CAP_IMAGEADDRESS_D
// Family:      Prism, Wildfire
// Type:        TWTY_STR255
// Container:   OneValue
// Allowed:     The image address template fields...
// Default:     (see CAP_IMAGEADDRESSTEMPLATES)
// Notes:       800 only.  Specifies the meaning and the maximum sizes
//              of the fields in the image address.  A value of 'f'
//              indicates a fixed field.  Values of 1, 2, 3 indicate
//              level fields.  The number of characters determines the
//              maximum allowed in that field.
#define CAP_IMAGEADDRESS_A                  0x804A
#define CAP_IMAGEADDRESS_B                  0x804B
#define CAP_IMAGEADDRESS_C                  0x804C
#define CAP_IMAGEADDRESS_D                  0x804D


// CAP_IMAGEMERGE
// Family:      A2O2, Falcon, Fosters, Panther, Piranha, Piranha2, Rufous
// Type:        TW_UINT16
// Container:   Enumeration
// Allowed:     TWIM_NONE, TWIM_FRONTONTOP, TWIM_FRONTONBOTTOM,
//              TWIM_FRONTONLEFT, TWIM_FRONTONRIGHT
// Default:     TWIM_NONE
// Notes:       Merges the front and back images into a single image that is 
//              returned to the application.
#define CAP_IMAGEMERGE                      0x80C5
#define    TWIM_NONE                        0
#define    TWIM_FRONTONTOP	                1	// front is on top of the back
#define    TWIM_FRONTONBOTTOM               2	// back is on top of front
#define    TWIM_FRONTONLEFT	                3	// front is to the left of the back
#define    TWIM_FRONTONRIGHT                4	// back is to the left of the front


// CAP_IMAGESDIFFERENT
// Family:      Blaze, Falcon, Panther, Piranha2
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       When this is TRUE the user can set different values for the
//              color and bitonal cameras.  When set to FALSE a value set
//				on a color camera will be matched (if possible) by the bitonal
//				camera, and vice versa.
#define CAP_IMAGESDIFFERENT                  0x8100


// CAP_INDICATORSWARMUP
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, Mustang2, 
//              Panther, Phoenix, Piranha, Piranha2, Pony, Rufous,
//              Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Controls the appearance of the LampSaver and Warmup
//              dialogs.  Only turn this off if these dialogs are
//              interfering with the operation of your application.
//              For version 9.3 drivers and up, if lamps are not
//              warmed-up at scan time, your application can be told
//              the number of seconds remaining before scanning will
//              begin; refer to TWDE_LAMPWARMUP in CAP_DEVICEEVENT
//              for more information.
#define CAP_INDICATORSWARMUP                0x806C


// CAP_INTELLIGENTDOCUMENTPROTECTION
// Family:      Falcon
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     See below...
// Default:     TWIDP_NORMAL
// Notes:       ADF only. Controls the level of document protection.
#define CAP_INTELLIGENTDOCUMENTPROTECTION	0x810D
#define    TWIDP_NONE                        0
#define    TWIDP_MINIMUM                     1
#define    TWIDP_NORMAL                      2
#define    TWIDP_MAXIMUM                     3


// CAP_LEVELTOFOLLOW1
// CAP_LEVELTOFOLLOW2
// CAP_LEVELTOFOLLOW3
// Family:      Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     TWPL_LEVEL1, TWPL_LEVEL2, TWPL_LEVEL3
// Default:     TWPL_LEVEL1
// Notes:       For Image Addressing, determines dependencies among the
//              level counters.
#define CAP_LEVELTOFOLLOW1                  0x803B
#define CAP_LEVELTOFOLLOW2                  0x803C
#define CAP_LEVELTOFOLLOW3                  0x803D


// CAP_MODE
// Family:      Gemini
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     TWMO_MODE1
// Notes:       Selects the current mode.
#define CAP_MODE                            0x8019
#define   TWMO_MODE1                        1
#define   TWMO_MODE2                        2
#define   TWMO_MODE3                        3
#define   TWMO_MODE4                        4
#define   TWMO_MODE5                        5
#define   TWMO_MODE6                        6
#define   TWMO_MODE7                        7
#define   TWMO_MODE8                        8
#define   TWMO_MODE9                        9
#define   TWMO_MODE10                       10
#define   TWMO_MODE11                       11
#define   TWMO_MODE12                       12
#define   TWMO_MODE13                       13
#define   TWMO_MODE14                       14
#define   TWMO_MODE15                       15
#define   TWMO_MODE16                       16
#define   TWMO_MODE17                       17
#define   TWMO_MODE18                       18


// CAP_MULTIFEEDCOUNT
// Family:      A2O2, Falcon, Fosters, Blaze, Inferno, Mustang2, Panther, Phoenix, 
//              Piranha, Piranha2, Pony, Rufous, Wildfire
// Type:        TWTY_INT32
// Container:   OneValue
// Allowed:     0 - 32767
// Default:     0
// Notes:       Count of multifeeds per scan session
#define CAP_MULTIFEEDCOUNT                  0x8086


// CAP_MULTIFEEDRESPONSE
// Family:      A2O2, Falcon, Fosters, Blaze, Inferno, Panther, Phoenix, Piranha, 
//              Piranha2, Rufous, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     Fosters - TWMR_CONTINUE, TWMR_ENDOFJOB
//              Inferno/Phoenix/Wildfire - TWMR_CONTINUE, TWMR_ENDOFJOB, TWMR_STOPFEEDER 
//              A2O2/Piranha - TWMR_CONTINUE, TWMR_ENDOFJOB, TWMR_ENDOFJOBLEAVEPAPER
//              Piranha2/Rufous - TWMR_CONTINUE, TWMR_ENDOFJOB, TWMR_ENDOFJOBLEAVEPAPER,
//                                TWMR_ENDOFJOBGENERATEIMAGE
//              Blaze/Falcon/Panther - TWMR_CONTINUE, TWMR_ENDOFJOB, TWMR_ENDOFJOBLEAVEPAPER, 
//                                     TWMR_ENDOFJOBGENERATEIMAGE, TWMR_STOPFEEDER, 
//                                     TWMR_STOPFEEDERLEAVEPAPER
// Default:     TWMR_ENDOFJOB
// Notes:       Selects action taken when the multifeed detected.
#define CAP_MULTIFEEDRESPONSE                0x80BA
#define    TWMR_CONTINUE                     0
#define    TWMR_ENDOFJOB                     1
#define    TWMR_ENDOFJOBLEAVEPAPER           2
#define    TWMR_STOPFEEDER                   3
#define    TWMR_STOPFEEDERLEAVEPAPER         4
#define    TWMR_ENDOFJOBGENERATEIMAGE        5


// CAP_MULTIFEEDSOUND
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, Mustang2, 
//              Panther, Phoenix, Piranha, Piranha2, Pony, Rufous,
//              Wildfire
// Type:        TWTY_STR255
// Container:   OneValue
// Allowed:     Text
// Default:     "ding.wav"
// Notes:       Sound when detect document multifeeds.
#define CAP_MULTIFEEDSOUND                  0x802D


// CAP_MULTIFEEDTHICKNESSDETECTION
// Family:      Viper (not 3500)
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Controls the whether thickness is used to detect
//              document multifeeds.
#define CAP_MULTIFEEDTHICKNESSDETECTION     0x8057


// CAP_NOWAIT
// Family:      A2O2, Alf, Alien, Blaze, Gemini, Falcon, Fosters, Inferno, 
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, Prism, 
//              Rufous, Wildfire, Viper
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Legacy behavior for this Source is to wait inside
//              of the MSG_ENDXFER command until the next image
//              is scanned or until the session is terminated, so
//              that it can properly set the value of the
//              TW_PENDINGXFERS.Count field.  This has the unhappy
//              side-effect of hanging the application.
//
//              This capability allows the application to issue a
//              non-blocking call to MSG_ENDXFER.  The application
//              must NOT move on to the next image until it receives
//              a TWRC_SUCCESS from the command.  A custom return
//              code of TWRC_BUSY will indicate that the Source
//              is still waiting on the device.  The application can
//              end the session by issuing a MSG_STOPFEEDER command.
//
//              Note that if CAP_AUTOSCAN is off then this situation
//              can happen for any image related command that is
//              issued:  DAT_IMAGEINFO, DAT_IMAGELAYOUT,
//              DAT_IMAGEFILEXFER, DAT_IMAGEMEMXFER, DAT_IMAGEFILEXFER.
//
//              TWAIN 2.2 defines TWRC_BUSY.  The driver assumes that
//              the application's TWAIN protocol corresponds to the
//              twain.h file that it was built with.  Therefore, for
//              applications that report a protocol of 2.1 or less
//              the value of TWRC_BUSY will be 0x8001.  For apps that
//              report a value of 2.2 or higher the value returned
//              will match whatever TWRC_BUSY is set to in the twain.h
//              file.  OBS_TWRC_BUSY is provided for situations where
//              an app reports a value of 2.1 or less when it is using
//              a 2.2 or greater twain.h.  In this case the application
//              must make a code change to use OBS_TWRC_BUSY.
//
//              Applications must also allow for the protocol reported
//              by the TWAIN driver.  Drivers reporting 2.1 or less
//              will only report a value of 0x8001 for TWRC_BUSY,
//              regardless of the protocol reported by the application.
#define CAP_NOWAIT                          0x8032
#define     OBS_TWRC_BUSY                   0x8001
#ifndef TWRC_BUSY
#define		TWRC_BUSY						0x8001
#endif


// CAP_PAGECOUNT
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, 
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, 
//              Prism, Rufous, Wildfire, Viper
// Type:        TWTY_FIX32
// Container:   Range
// Allowed:     0 - 32767
// Default:     0
// Notes:       Count of pages to transfer.  This capability only
//              takes effect if CAP_XFERCOUNT is -1.  A value of 0
//              disables this capability.
#define CAP_PAGECOUNT                       0x8031


// CAP_PAGESIZELIMIT
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno,  
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, 
//              Prism, Rufous, Wildfire, Viper
// Type:        TWTY_FIX32
// Container:   Range
// Allowed:     0 - ICAP_PHYSICALHEIGHT
// Default:     0
// Notes:       Maximum allowed physical page height (in ICAP_UNITS).
//              This is a double document detection feature.
#define CAP_PAGESIZELIMIT                   0x8002


// CAP_PAPERSOURCE
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, 
//              Mustang2,  Panther, Phoenix, Piranha, Piranha2, Pony, 
//              Rufous, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     A2O2, Alien, Piranha, Piranha2 - TWPU_ADF.  If platen installed
//              TWPU_AUTO and TWPU_PLATEN also allowed.
//              Alf, Fosters, Pony, Rufous - TWPU_ADF.
//              Mustang2 - TWPU_ADF, TWPU_AUTO, TWPU_PLATEN
//              Inferno/Panther/Phoenix/Wildfire - TWPU_ADF, TWPU_ELEVATOR100,
//              TWPU_ELEVATOR250, TWPU_ELEVATOR500
//              Blaze - TWPU_ADF, TWPU_ELEVATOR100, TWPU_ELEVATOR250,
//              TWPU_ELEVATOR500, TWPU_ELEVATOR750
//              Falcon (if only ADF) - TWPU_ADF, TWPU_ELEVATOR100, TWPU_ELEVATOR250
//              Falcon (if Flatbed available) - add TWPU_AUTO, TWPU_PLATEN,
//              TWPU_ELEVATOR100PLATEN,  TWPU_ELEVATOR250PLATEN
// Default:     Alf, Alien, Fosters, Pony, Rufous - TWPU_ADF
//              Inferno/Panther/Phoenix/Wildfire - TWPU_ELEVATOR500
//              Blaze - TWPU_ELEVATOR750
//              A2O2, Mustang2, Piranha, Piranha2 - TWPU_AUTO
//              Falcon (if only ADF) - TWPU_ELEVATOR250
//              Falcon (if Flatbed available) - TWPU_ELEVATOR250PLATEN
// Notes:       Selects source of paper (ADF or Platen).  TWPU_AUTO
//              selects ADF but changes to Platen if the ADF has no
//              paper in it at the start of the scanning session.
#define CAP_PAPERSOURCE                     0x802C
#define    TWPU_AUTO                        0
#define    TWPU_ADF                         1
#define    TWPU_PLATEN                      2
#define    TWPU_ELEVATOR500                 3
#define    TWPU_ELEVATOR250                 4
#define    TWPU_ELEVATOR100                 5
#define    TWPU_ELEVATOR750                 6
#define    TWPU_ELEVATOR100PLATEN           7
#define    TWPU_ELEVATOR250PLATEN           8
#define    TWPU_ELEVATOR500PLATEN           9


// CAP_PATCHCOUNT
// Family:      A2O2 (i1440), Alien(i280), Inferno, Panther, Phoenix, 
//              Wildfire
// Type:        TWTY_INT32
// Container:   OneValue
// Allowed:     0 - 32767
// Default:     0
// Notes:       Count of patches per scan session
#define CAP_PATCHCOUNT                      0x8087


// CAP_PATCHHEAD1
// Family:      Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Enables/disables the first patch reader.
#define CAP_PATCHHEAD1                    0x80BF


// CAP_PATCHHEAD2
// Family:      Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Enables/disables the second patch reader.
#define CAP_PATCHHEAD2                    0x80C0


// CAP_PATCHHEAD3
// Family:      Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Enables/disables the third patch reader.
#define CAP_PATCHHEAD3                    0x80C1


// CAP_PATCHHEAD4
// Family:      Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Enables/disables the fourth patch reader.
#define CAP_PATCHHEAD4                    0x80C2


// CAP_PCARDENABLED
// Family:      n/a
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Enables/disables the P-Card (note that this operates
//              in tandem with the DAT_PCARD operation, setting this
//              value here or there will modify both values).
//              This capability is not available at this time.
#define CAP_PCARDENABLED                    0x806A


// CAP_POWEROFFTIMEOUT
// Family:      Falcon
// Type:        TWTY_INT32
// Container:   Range
// Allowed:     Falcon: 0, 0 - 240 (minutes)
// Default:     60
// Notes:       Minutes of idle time following CAP_ENERGYSTAR
//              before device is powered off...
#define CAP_POWEROFFTIMEOUT                      0x810C


// CAP_PRINTERDATE
// Family:      Blaze, Falcon, Inferno, Panther, Phoenix, Wildfire
// Type:        TWTY_STR255
// Container:   OneValue
// Allowed:     Text
// Default:     ""
// Notes:       This will set the scanner to a specific date prior to printing.
//              The scanner will be returned to the original date afterwards.
//		        Set to "" to disable.
//		        Format:		YYYY/MM/DD		(YYYY - year, MM - month, DD - day)
#define CAP_PRINTERDATE                     0x80BC


// CAP_PRINTERDATEDELIMITER
// Family:      A2O2, Alien, Blaze, Falcon, Inferno, Panther, Phoenix, Prism, 
//              Wildfire, Viper (3520/4500)  
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     Prism - TWPD_FORWARDSLASH
//              Others - TWPD_NONE
// Notes:       Selects the delimiter to be used in the date
//              (not the time, though, time always uses a
//              delimiter of colon ':').
#define CAP_PRINTERDATEDELIMITER            0x801C
#define   TWPD_NONE                         0
#define   TWPD_FORWARDSLASH                 1
#define   TWPD_HYPHEN                       2
#define   TWPD_PERIOD                       3
#define   TWPD_BLANK                        4


// CAP_PRINTERDATEFORMAT
// Family:      A2O2, Blaze, Falcon, Inferno, Panther, Phoenix, Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     A2O2/Phoenix/Prism - TWPE_MMDDYYYY, TWPE_DDMMYYYY, TWPE_YYYYMMDD
//				Other - TWPE_MMDDYYYY, TWPE_DDMMYYYY, TWPE_YYYYMMDD, TWPE_DDD, TWPE_YYYYDDD
// Default:     Prism - TWPE_MMDDYYYY
//              Other - TWPE_YYYYMMDD
// Notes:       Selects the format to be used to display
//              the date (not the time, though).
#define CAP_PRINTERDATEFORMAT               0x8033
#define   TWPE_MMDDYYYY                     0
#define   TWPE_DDMMYYYY                     1
#define   TWPE_YYYYMMDD                     2
#define   TWPE_DDD                          3
#define   TWPE_YYYYDDD                      4


// CAP_PRINTERFONT
// Family:      A2O2, Alien, Blaze, Falcon, Gemini, Inferno, Panther Phoenix, 
//              Prism, Wildfire, Viper (3520/4500)  
// Type:        TWTY_UINT16
// Allowed:     A2O2/Alien/Gemini/Viper - TWPN_LARGECOMIC,TWPN_LARGECINE,TWPN_SMALLCOMIC,TWPN_SMALLCINE
//              Inferno/Phoenix/Prism/Wildfire - TWPN_LARGECOMIC,TWPN_LARGECINE,TWPN_LARGECOMIC180,TWPN_LARGECINE180,
//                                               TWPN_SMALLCOMIC,TWPN_SMALLCINE,TWPN_SMALLCOMIC180,TWPN_SMALLCINE180
//              Blaze/Falcon - TWPN_LARGECOMIC,TWPN_LARGECINE,TWPN_LARGECOMIC180,TWPN_LARGECINE180,
//                      TWPN_SMALLCOMIC,TWPN_SMALLCINE,TWPN_SMALLCOMIC180,TWPN_SMALLCINE180,
//                      TWPN_BOLDLARGECOMIC,TWPN_BOLDLARGECINE,TWPN_BOLDLARGECOMIC180,TWPN_BOLDLARGECINE180
//              Panther - TWPN_LARGECOMIC,TWPN_LARGECINE,TWPN_LARGECOMIC180,TWPN_LARGECINE180,
//						  TWPN_SMALLCOMIC,TWPN_SMALLCINE,TWPN_SMALLCOMIC180,TWPN_SMALLCINE180,
//                        TWPN_BOLDLARGECOMIC,TWPN_BOLDLARGECINE
// Default:     Blaze, Falcon, Panther, Prism-   TWPN_SMALLCOMIC
//              Other-   TWPN_LARGECOMIC
//              
//              
// Notes:       elects the printer font to be used.
//				Small also means 'Normal'; Large also means 'Bold'; BoldLarge also means 'ExtraBold'
#define CAP_PRINTERFONT                     0x8044
#define   TWPN_LARGECOMIC                   0
#define   TWPN_LARGECINE                    1
#define   TWPN_LARGECOMIC180                2
#define   TWPN_LARGECINE180                 3
#define   TWPN_SMALLCOMIC                   4
#define   TWPN_SMALLCINE                    5
#define   TWPN_SMALLCOMIC180                6
#define   TWPN_SMALLCINE180                 7
#define   TWPN_BOLDLARGECOMIC               8
#define   TWPN_BOLDLARGECINE                9
#define   TWPN_BOLDLARGECOMIC180            10
#define   TWPN_BOLDLARGECINE180             11
#define   TWPN_BOLDSMALLCOMIC               12
#define   TWPN_BOLDSMALLCINE                13
#define   TWPN_BOLDSMALLCOMIC180            14
#define   TWPN_BOLDSMALLCINE180             15


// CAP_PRINTERFONTFORMAT
// Family:      Wildfire
// Type:        TWTY_UINT16
// Allowed:     TWPFF_NORMAL, TWPFF_BLOCK
// Default:     TWPFF_NORMAL
//              
// Notes:       elects the printer font format to be used
#define   CAP_PRINTERFONTFORMAT             0x80BE
#define   TWPFF_NORMAL                      0
#define   TWPFF_BLOCK                       1


// CAP_PRINTERIMAGEADDRESSFORMAT
// Family:      Gemini, Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     Gemini-TWPI_DISPLAYLEADINGZEROS
//              Prism-TWPI_SUPPRESSLEADINGZEROS
//              Wildfire-TWPI_SUPPRESSLEADINGZEROS
// Notes:       Selects the print format of leading zeros
//              in the image address.
#define CAP_PRINTERIMAGEADDRESSFORMAT       0x8045
#define   TWPI_DISPLAYLEADINGZEROS          0
#define   TWPI_SUPPRESSLEADINGZEROS         1
#define   TWPI_COMPRESSLEADINGZEROS         2


// CAP_PRINTERIMAGEADDRESSLEVEL
// Family:      Gemini, Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     Gemini-TWPL_LEVEL0, TWPL_LEVEL1, TWPL_LEVEL2,
//                     TWPL_LEVEL3, TWPL_ALLLEVELS
//              Prism- TWPL_LEVEL1, TWPL_LEVEL2, TWPL_LEVEL3,
//                       TWPL_ALLLEVELS
//              Wildfire-TWPL_LEVEL1, TWPL_LEVEL2, TWPL_LEVEL3,
//                     TWPL_ALLLEVELS
// Default:     TWPL_ALLLEVELS
// Notes:       Selects the image address level that printing
//              will occur on.  A value of TWPL_ALLLEVELS
//              overrides any of the others.
#define CAP_PRINTERIMAGEADDRESSLEVEL        0x8047
#define   TWPL_LEVEL0                       0
#define   TWPL_LEVEL1                       1
#define   TWPL_LEVEL2                       2
#define   TWPL_LEVEL3                       3
#define   TWPL_ALLLEVELS                    4
#define   TWPL_NONE                         5


// CAP_PRINTERINDEXDIGITS
// Family:      A2O2, Alien, Blaze, Falcon, Inferno, Panther, Phoenix, 
//              Prism, Wildfire, Viper (3520/4500)
// Type:        TWTY_UINT16
// Container:   Range
// Allowed:     1 - 9
// Default:     9
// Notes:       Sets the number of digits of the counter to be
//              printed.  Note that the data will be truncated
//              if the number of digits in the count exceeds
//              this value.
#define CAP_PRINTERINDEXDIGITS              0x801B


// CAP_PRINTERINDEXFORMAT
// Family:      A2O2, Alien, Blaze, Falcon, Inferno, Panther, Phoenix, 
//              Prism, Wildfire, Viper (3520/4500)
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see CAP_PRINTERIMAGEADDRESSFORMAT)
// Default:     Prism-TWPI_SUPPRESSLEADINGZEROS
//              Other-TWPI_DISPLAYLEADINGZEROS
// Notes:       Selects the use of leading zeros or spaces when
//              printing the counter value.
#define CAP_PRINTERINDEXFORMAT              0x801A


// CAP_PRINTERPOSITION
// Family       A2O2, Alien, Blaze, Falcon, Gemini, Inferno, Panther, Phoenix, 
//              Prism, Wildfire, Viper (3520/4500) 
// Type:        TWTY_FIX32
// Container:   Range
// Allowed:     Scanner specific
// Default:     Scanner specific
// Notes:       Selects the printer position (y-offset, in ICAP_UNITS)
//              to be used as the starting position for printing.
#define CAP_PRINTERPOSITION                 0x8046


// CAP_PRINTERTIME
// Family:      Inferno, Blaze, Falcon, Panther, Phoenix, Wildfire
// Type:        TWTY_STR255
// Container:   OneValue
// Allowed:     Text
// Default:     ""
// Notes:       This will set the scanner to a specific time prior to printing.
//              The scanner will be returned to the original time afterwards.
//		        Set to "" to disable.
//		        Format:		HH:MM		(HH - hour (0-23), MM - minutes (0-59),
//              i.e. 00:00->23:59)
#define CAP_PRINTERTIME                     0x80BD


// CAP_PRINTERWRITESEQUENCE
// Family:      A2O2, Alien, Blaze, Falcon, Gemini(5000/7000), Inferno, Panther, 
//              Phoenix, Prism, Wildfire
// Type:        TWTY_UINT16
// Allowed:     All the stuff below (sheesh)...
// Notes:       Selects the write sequence.
#define CAP_PRINTERWRITESEQUENCE            0x8049
#define   TWPW_NONE                         0
#define   TWPW_FULLIAALLDELIMITERS          1
#define   TWPW_FULLIANODELIMITERS           2
#define   TWPW_FULLIAFIXEDDELIMITERS        3
#define   TWPW_DELIMITER                    4
#define   TWPW_FIXEDIA                      5
#define   TWPW_INDEXIA                      6
#define   TWPW_CURRENTLEVELIA               7
#define   TWPW_DATE                         8
#define   TWPW_DAY                          9
#define   TWPW_TIME                         10
#define   TWPW_DOCUMENTCOUNT                11
#define   TWPW_MESSAGE1                     12
#define   TWPW_MESSAGE2                     13
#define   TWPW_MESSAGE3                     14
#define   TWPW_MESSAGE4                     15
#define   TWPW_MESSAGE5                     16
#define   TWPW_MESSAGE6                     17
#define   TWPW_MESSAGE7                     18
#define   TWPW_MESSAGE8                     19
#define   TWPW_MESSAGE9                     20
#define   TWPW_SPACE                        21
#define   TWPW_TEXT                         22
#define   TWPW_INDEX                        23
#define   TWPW_MMDDYYYY                     24
#define   TWPW_DDMMYYYY                     25
#define   TWPW_YYYYMMDD                     26
#define   TWPW_IMAGEADDRESS_A               27
#define   TWPW_IMAGEADDRESS_B               28
#define   TWPW_IMAGEADDRESS_C               29
#define   TWPW_IMAGEADDRESS_D               30


// CAP_PRINTERWRITESEQUENCEINDEX
// Family:      Gemini (5000/7000)
// Type:        TWTY_UINT16
// Allowed:     1 - 7
// Notes:       Selects the write sequence index.
#define CAP_PRINTERWRITESEQUENCEINDEX       0x8048


// CAP_PRINTERWRITESEQUENCEMESSAGE1 - 12
// Family:      A2O2, Blaze, Falcon, Gemini, Inferno, Panther, Phoenix, 
//              Prism, Wildfire, Viper (3520/4500) 
// Type:        TWTY_STR255
// Container:   OneValue
// Allowed:     A2O2/Falcon/Panther only has one message
//              Prism/Phoenix/Wildfire/Inferno: 1 to 6
// Default:     A2O2- ABC
//              Gemini-#1
//              Viper- ABC
//              Prism- (empty string)
//              Inferno/Phoenix- ABC
//              Blaze/Falcon/Panther/Wildfire- 111
// Notes:       5000/7000 only...selects the write sequence
//              messages (1 - 9).  Prism only...selects the
//              write sequence messages 1-6 if the levels
//              are independent and 7-12 for 'all' levels.
#define CAP_PRINTERWRITESEQUENCEMESSAGE1    0x800C
#define CAP_PRINTERWRITESEQUENCEMESSAGE2    0x800D
#define CAP_PRINTERWRITESEQUENCEMESSAGE3    0x800E
#define CAP_PRINTERWRITESEQUENCEMESSAGE4    0x800F
#define CAP_PRINTERWRITESEQUENCEMESSAGE5    0x8010
#define CAP_PRINTERWRITESEQUENCEMESSAGE6    0x8011
#define CAP_PRINTERWRITESEQUENCEMESSAGE7    0x8012
#define CAP_PRINTERWRITESEQUENCEMESSAGE8    0x8013
#define CAP_PRINTERWRITESEQUENCEMESSAGE9    0x8014
#define CAP_PRINTERWRITESEQUENCEMESSAGE10   0x8034
#define CAP_PRINTERWRITESEQUENCEMESSAGE11   0x8035
#define CAP_PRINTERWRITESEQUENCEMESSAGE12   0x8036


// CAP_PRINTERWRITESEQUENCEMESSAGEINDEX
// Family:      A2O2, Alien, Inferno, Phoenix, Wildfire
// Type:        TWTY_UINT16
// Container:   OneValue
// Allowed:     Inferno/Phoenix/Wildfire 1 - 6
//              A2O2, Alien   1
// Default:     1
// Notes:       Selects the write sequence messages.
#define CAP_PRINTERWRITESEQUENCEMESSAGEINDEX  0x808A


// CAP_PRINTERWRITESEQUENCESPACE
// Family:      Gemini(5000/7000)
// Type:        TWTY_UINT16
// Allowed:     Number of blanks...
// Notes:       5000/7000 only.  Blank count for the write sequence.
#define CAP_PRINTERWRITESEQUENCESPACE       0x800A


// CAP_PRINTERWRITESEQUENCESPACESTRING
// Family:      Gemini(5000/7000)
// Type:        TWTY_STR255
// Allowed:     The WriteSequenceSpace string...
// Notes:       5000/7000 only.  Selects the write sequence
//              count of spaces.
#define CAP_PRINTERWRITESEQUENCESPACESTRING 0x800B


// CAP_PRINTERWRITESEQUENCESTRING
// Family:      A2O2, Alien, Blaze, Falcon, Gemini (5000/7000), Inferno, 
//              Panther, Phoenix, Prism, Wildfire
// Type:        TWTY_STR255
// Allowed:     The WriteSequence string...
// Notes:       5000/7000 only.  Selects the write sequence.
#define CAP_PRINTERWRITESEQUENCESTRING      0x8009


// CAP_PROFILES
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Panther, Phoenix, Piranha,
//              Piranha2, Rufous, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     TWPO_DEFAULT
// Notes:       THIS IS DEPRECATED TWAIN 10.X+. DAT_PROFILES SHOULD BE USED INSTEAD.
//              Selects the current profile. This set the current settings from
//              the selected profile to the driver.
//              Or see the CUSTOMDSDATA that works the same as profile.
//              See the MSG_SETUPDS on how to download the settings to the scanner
//              without enable the scanning session.
//              Also refer to DAT_PROFILES later in this file.
#define CAP_PROFILES                        0x809F
#define    TWPO_UNKNOWN                     -1
#define    TWPO_DEFAULT                     0
#define    TWPO_FILE01                      1
#define    TWPO_FILE02                      2
#define    TWPO_FILE03                      3
#define    TWPO_FILE04                      4
#define    TWPO_FILE05                      5
#define    TWPO_FILE06                      6
#define    TWPO_FILE07                      7
#define    TWPO_FILE08                      8
#define    TWPO_FILE09                      9
#define    TWPO_FILE10                      10
#define    TWPO_FILE11                      11
#define    TWPO_FILE12                      12
#define    TWPO_FILE13                      13
#define    TWPO_FILE14                      14
#define    TWPO_FILE15                      15
#define    TWPO_FILE16                      16
#define    TWPO_FILE17                      17
#define    TWPO_FILE18                      18
#define    TWPO_FILE19                      19
#define    TWPO_FILE20                      20
#define    TWPO_FILE21                      21
#define    TWPO_FILE22                      22
#define    TWPO_FILE23                      23
#define    TWPO_FILE24                      24
#define    TWPO_FILE25                      25
#define    TWPO_FILE26                      26
#define    TWPO_FILE27                      27
#define    TWPO_FILE28                      28
#define    TWPO_FILE29                      29
#define    TWPO_FILE30                      30
#define    TWPO_FILE31                      31
#define    TWPO_FILE32                      32
#define    TWPO_FILE33                      33
#define    TWPO_FILE34                      34
#define    TWPO_FILE35                      35
#define    TWPO_FILE36                      36
#define    TWPO_FILE37                      37
#define    TWPO_FILE38                      38
#define    TWPO_FILE39                      39
#define    TWPO_FILE40                      40
#define    TWPO_FILE41                      41
#define    TWPO_FILE42                      42
#define    TWPO_FILE43                      43
#define    TWPO_FILE44                      44
#define    TWPO_FILE45                      45
#define    TWPO_FILE46                      46
#define    TWPO_FILE47                      47
#define    TWPO_FILE48                      48
#define    TWPO_FILE49                      49
#define    TWPO_FILE50                      50
#define    TWPO_FILE51                      51
#define    TWPO_FILE52                      52
#define    TWPO_FILE53                      53
#define    TWPO_FILE54                      54
#define    TWPO_FILE55                      55
#define    TWPO_FILE56                      56
#define    TWPO_FILE57                      57
#define    TWPO_FILE58                      58
#define    TWPO_FILE59                      59
#define    TWPO_FILE60                      60


// CAP_SIDESDIFFERENT
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Panther, Phoenix, Piranha,
//              Piranha2, Rufous, Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       When this is TRUE the user can set different values for the
//              front and rear camera.
//              Setting this to FALSE will copy all the front settings to the
//              rear for the current camera.
//              This will automatically change to TRUE when the front and
//              rear no longer match.  Note: a value of TRUE does NOT imply
//              that the sides are different (e.g. if everything matches and
//              this is set to TRUE)
#define CAP_CAMERALINK	                    0x80B7 // deprecated after v7.55
#define CAP_SIDESDIFFERENT                  0x80B7 // available since v7.56


// CAP_SIMULATING
// Family:      Blaze, Falcon, Panther, Piranha2
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Simulate the scanner.
#define CAP_SIMULATING                      0x810B


// CAP_SUPPORTEDSIZES
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Gemini, Inferno, 
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, Prism, 
//              Rufous, Wildfire, Viper
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     TWSS_A4
// Notes:       This list adds custom values to this standard TWAIN
//              capability.  The TWSS_USER* values may be set up by
//              an end-user through the CONST.INI and appropriate
//              language file(s) to add custom sizes to the existing
//              list.  See the CONST.INI file under the section
//              [SupportedSizes] for more info on how to do this.
#define TWSS_6X5                            0x8001
#define TWSS_12X12                          0x8002
#define TWSS_3X5                            0x8003        // 3.5 x 5
#define TWSS_4X6                            0x8004
#define TWSS_5X7                            0x8005
#define TWSS_8X10                           0x8006
#define TWSS_4X7                            0x8007
#define TWSS_4X10                           0x8008
#define TWSS_100X150                        0x8009
#define TWSS_127X177                        0x8010
#define TWSS_90X130                         0x8011
#define TWSS_57X88                          0x8012        // 57.17 x 88.9
#define TWSS_5X3                            0x8013        // 3.5 x 5
#define TWSS_6X4                            0x8014
#define TWSS_7X5                            0x8015
#define TWSS_7X4                            0x8016
#define TWSS_150X100                        0x8017
#define TWSS_177X127                        0x8018
#define TWSS_130X90                         0x8019
#define TWSS_88X57                          0x8020        // 57.17 x 88.9
#define TWSS_USER0                          0x8100
#define TWSS_USER1                          0x8101
#define TWSS_USER2                          0x8102
#define TWSS_USER3                          0x8103
#define TWSS_USER4                          0x8104
#define TWSS_USER5                          0x8105
#define TWSS_USER6                          0x8106
#define TWSS_USER7                          0x8107
#define TWSS_USER8                          0x8108
#define TWSS_USER9                          0x8109


// CAP_TOGGLEPATCH
// Family:      A2O2 (i1440), Alien (i280), Blaze, Falcon (i3000), Inferno, 
//              Panther,  Phoenix, Prism, Wildfire 
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     TWTP_DISABLED
// Notes:       Controls recognition of the color patch.
#define CAP_TOGGLEPATCH                     0x806D
#define   TWTP_DISABLED                     0
#define   TWTP_BOTHSIDE                     1
#define   TWTP_FRONTSIDE                    2
#define   TWTP_SAMESIDE                     4
#define   TWTP_DETECTONLY                   7


// CAP_TRANSPORTAUTOSTART
// Family:      A2O2, Fosters, Blaze, Inferno, Panther, Phoenix, 
//              Piranha, Piranha2, Prism, Rufous, Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     A2O2, Fosters, Piranha, Piranha2, Rufous: TRUE (readonly)
//              Other: TRUE / FALSE
// Default:     A2O2, Blaze, Fosters, Panther, Piranha, Piranha2, Rufous: TRUE
//              Other: FALSE
// Notes:       A value of TRUE will cause the scanner transport to
//              start up when the MSG_ENABLEDS command is issued.
#define CAP_TRANSPORTAUTOSTART              0x8029


// CAP_TRANSPORTTIMEOUT
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, 
//              Mustang2, Panther,Phoenix, Piranha, Piranha2, Pony, 
//              Prism, Rufous, Wildfire, Viper
// Type:        TWTY_INT32
// Container:   Range
// Allowed:     Alf/Alien: 3-30
//              Mustang2/Pony: 1-30
//              A2O2/Fosters/Inferno/Phoenix/Piranha/Wildfire: 0,1-300
//              Prism: 0,5-300
//              Viper: 3-30
//              Blaze/Panther/Piranha2/Rufous: 0, 1-120
// Default:     A2O2/Alf/Alien: 8
//              Fosters/Mustang2/Piranha/Pony: 1
//              Inferno/Phoenix/Wildfire: 15
//              Blaze/Panther: 5
//              Prism: 10
//              Viper: 8
//              Piranha2/Rufous: 0
// Notes:       Number of seconds before transport times out.
#define CAP_TRANSPORTTIMEOUT                0x8003


// CAP_TRANSPORTTIMEOUTRESPONSE
// Family:      A2O2, Fosters, Blaze, Falcon, Inferno, Panther, Phoenix, 
//              Piranha, Piranha2, Prism, Rufous, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     A2O2, Fosters, Piranha, Piranha2, Rufous: TWTR_ENDOFJOB
//				Other: See below
// Default:     A2O2,Falcon,Fosters,Panther,Piranha,Piranha2,Rufous: TWTR_ENDOFJOB
//              Other: TWTR_STOPFEEDER
// Notes:       Selects action taken when the transport times out.
//              If the transport timeout is disabled, then this
//              capability is ignored.
#define CAP_TRANSPORTTIMEOUTRESPONSE        0x8028
#define    TWTR_STOPFEEDER                  0
#define    TWTR_ENDOFJOB                    1


// CAP_ULTRASONICSENSITIVITY
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Inferno, Panther, Phoenix,  
//              Piranha, Piranha2, Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     See below...
// Default:     TWUS_DISABLED
// Notes:       Controls the sensitivity of the ultrasonic multifeed
//              detector.
//
//              Please note that there is a conflict with version 2.2 of the
//              TWAIN Spec.  Previous versions of kds_cust.h defined TWUS_*
//              values.  This behavior is still supported if used with
//              a TWAIN 2.1 twain.h or earlier.
//
//              Applications must change to use TWUSS_* with CAP_ULTRASONICSENSITIVITY,
//              this will work with any version of the driver.
//
//              Only use TWUS_* with the standard CAP_DOUBLEFEEDDETECTIONSENSITIVITY
//              capability.
#define CAP_ULTRASONICSENSITIVITY           0x8027
#ifndef CAP_DOUBLEFEEDDETECTIONSENSITIVITY
	#define    TWUS_DISABLED                    0
	#define    TWUS_LOW                         1
	#define    TWUS_MEDIUM                      2
	#define    TWUS_HIGH                        3
#endif
#define    TWUSS_DISABLED                       0
#define    TWUSS_LOW                            1
#define    TWUSS_MEDIUM                         2
#define    TWUSS_HIGH                           3


// CAP_ULTRASONICSENSOR*
// Family:      Blaze, Inferno, Panther, Phoenix, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     See below...
// Default:     TWUO_ENABLED
// Notes:       Controls the individual ultrasonic sensor
//              detector.
#define CAP_ULTRASONICSENSORCENTER          0x808F
#define CAP_ULTRASONICSENSORLEFT            0x8090
#define CAP_ULTRASONICSENSORRIGHT           0x8091
#define CAP_ULTRASONICSENSORLEFTCENTER      0x8105
#define CAP_ULTRASONICSENSORRIGHTCENTER     0x8106
#define    TWUO_DISABLED                    0
#define    TWUO_ENABLED                     1
#define    TWUO_IGNOREZONE                  2


// CAP_ULTRASONICSENSORZONEHEIGHT
// Family:      Blaze
// Type:        TWTY_FIX32
// Container:   Range
// Allowed:     Scanner specific
// Default:     Scanner specific
// Notes:       Height of zone for the CAP_ULTRASONICSENSOR* that
//              are set to TWUO_IGNOREZONE
#define CAP_ULTRASONICSENSORZONEHEIGHT      0x8107


// CAP_WINDOW
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Panther, Phoenix, 
//              Piranha, Piranha2, Rufous, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     See below..
// Default:     TWWW_BASE
// Notes:       Deprecated, use CAP_WINDOWCAMERA or DAT_FILESYSTEM.
#define CAP_WINDOW                          0x80AD
#define    TWWW_BASE                        0
#define    TWWW_1                           1
#define    TWWW_2                           2
#define    TWWW_3                           3
#define    TWWW_4                           4
#define    TWWW_5                           5
#define    TWWW_6                           6
#define    TWWW_7                           7
#define    TWWW_8                           8
#define    TWWW_9                           9
#define    TWWW_10                          10


// CAP_WINDOWCAMERA
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Panther, Phoenix,
//              Piranha, Piranha2, Rufous, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     See below..
// Default:     TWWC_BASE_BITONAL_FRONT
// Notes:       Allows for direct addressing of windows.  Does not
//				change CAP_SIDESDIFFERENT like DAT_FILESYSTEM.
#define CAP_WINDOWCAMERA                    0x80B4
#define    TWWC_DELETEALL                   -1
#define    TWWC_BASE_BITONAL_FRONT          0
#define    TWWC_BASE_BITONAL_REAR           1
#define    TWWC_BASE_COLOR_FRONT            2
#define    TWWC_BASE_COLOR_REAR             3
#define    TWWC_1_BITONAL_FRONT             4
#define    TWWC_1_BITONAL_REAR              5
#define    TWWC_1_COLOR_FRONT               6
#define    TWWC_1_COLOR_REAR                7
#define    TWWC_2_BITONAL_FRONT             8
#define    TWWC_2_BITONAL_REAR              9
#define    TWWC_2_COLOR_FRONT               10
#define    TWWC_2_COLOR_REAR                11
#define    TWWC_3_BITONAL_FRONT             12
#define    TWWC_3_BITONAL_REAR              13
#define    TWWC_3_COLOR_FRONT               14
#define    TWWC_3_COLOR_REAR                15
#define    TWWC_4_BITONAL_FRONT             16
#define    TWWC_4_BITONAL_REAR              17
#define    TWWC_4_COLOR_FRONT               18
#define    TWWC_4_COLOR_REAR                19
#define    TWWC_5_BITONAL_FRONT             20
#define    TWWC_5_BITONAL_REAR              21
#define    TWWC_5_COLOR_FRONT               22
#define    TWWC_5_COLOR_REAR                23
#define    TWWC_6_BITONAL_FRONT             24
#define    TWWC_6_BITONAL_REAR              25
#define    TWWC_6_COLOR_FRONT               26
#define    TWWC_6_COLOR_REAR                27
#define    TWWC_7_BITONAL_FRONT             28
#define    TWWC_7_BITONAL_REAR              29
#define    TWWC_7_COLOR_FRONT               30
#define    TWWC_7_COLOR_REAR                31
#define    TWWC_8_BITONAL_FRONT             32
#define    TWWC_8_BITONAL_REAR              33
#define    TWWC_8_COLOR_FRONT               34
#define    TWWC_8_COLOR_REAR                35
#define    TWWC_9_BITONAL_FRONT             36
#define    TWWC_9_BITONAL_REAR              37
#define    TWWC_9_COLOR_FRONT               38
#define    TWWC_9_COLOR_REAR                39
#define    TWWC_10_BITONAL_FRONT            40
#define    TWWC_10_BITONAL_REAR             41
#define    TWWC_10_COLOR_FRONT              42
#define    TWWC_10_COLOR_REAR               43


// CAP_WINDOWPOSITION
// Family:      A2O2, Alf, Alien, Fosters, Gemini, Inferno, Mustang2, 
//              Phoenix, Piranha, Pony, Prism, Wildfire, Viper
// Type:        TWTY_FRAME
// Container:   OneValue
// Allowed:     Anything
// Default:     0,0 (top left of screen)
// Notes:       Left and Top position of dialog (Width and Height are
//              ignored).  Use this to select the location of the
//              Source's GUI when it is displayed to the user.
#define CAP_WINDOWPOSITION                  0x8004



////////////////////////////////////////////////////////////////////////////////
//                      ICAP Section
////////////////////////////////////////////////////////////////////////////////



// ICAP_ADDBORDER
// Family:      A2O2, Blaze, Falcon, Fosters, Inferno, Panther, Phoenix, 
//              Piranha, Piranha2, Prism, Rufous, Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Adds a black border around a bitonal document.
#define ICAP_ADDBORDER                      0x8023


#define ICAP_ANSELBRIGHTNESS                0x80A0
#define ICAP_ANSELCONTRAST                  0x80A1
#define ICAP_ANSELHIGHLIGHT                 0x80A2
#define ICAP_ANSELMIDTONE                   0x80A3


// ICAP_ANSELREMOVEREDEYE
// Family:      Oasis
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Controls red-eye removal.
#define ICAP_ANSELREMOVEREDEYE              0x80A4


// ICAP_ANSELRESTORECOLOR
// Family:      Oasis
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Adds a black border around a bitonal document.
#define ICAP_ANSELRESTORECOLOR              0x80A5


#define ICAP_ANSELSATURATECOLORS            0x80A6
#define ICAP_ANSELSHADOW                    0x80A7


// ICAP_ANSELSHARPENIMAGE
// Family:      Oasis
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     (see below)
// Default:     TWASP_DISABLE
// Notes:       Controls picture sharpening.
#define ICAP_ANSELSHARPENIMAGE              0x80A8
#define   TWASP_DISABLE                     0
#define   TWASP_NORMAL                      1
#define   TWASP_HIGH                        2
#define   TWASP_EXAGGERATED                 3


// ICAP_ANSELSHASTA
// Family:      Oasis
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     (see below)
// Default:     TWASH_DISABLE
// Notes:       TBD
#define ICAP_ANSELSHASTA                    0x80A9
#define   TWASH_DISABLE                     0
#define   TWASH_AUTO                        1
#define   TWASH_MANUAL                      2


// ICAP_AUTOCOLORAMOUNT
// Family:      A2O2, Blaze, Falcon, Inferno, Panther, Phoenix, Piranha, 
//              Piranha2, Wildfire
// Type:        TWTY_INT32
// Container:   Range (per camera)
// Allowed:     1 to 200
// Default:     1
// Notes:       The amount of color that needs to be present in a document
//              before it will be saved as either a color or grayscale image.
#define ICAP_AUTOCOLORAMOUNT                0x8092


// ICAP_AUTOCOLORCONTENT
// Family:      A2O2, Blaze, Falcon, Inferno, Panther, Phoenix, Piranha, 
//              Piranha2, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     TWCL_NONE,TWCL_LOW,TWCL_MEDIUM,TWCL_HIGH,TWCL_CUSTOM
// Default:     TWCL_NONE
// Notes:       Preset values for AUTOCOLORAMOUNT and AUTOCOLORTHRESHOLD.
//              NONE turns autocolor detection off.
//              CUSTOM allows amount and threshold to be set.
//              When this is not NONE, CAMERAENABLE has no effect on output
//              color (rgb/gray/bw).  Rather, CAMERAENABLE dictates which
//				sides should be scanned (front/rear).
//              Enabling both color and bitonal for a side indicates both are
//              desired as output, and therefore AUTOCOLORCONTENT will be set
//              to TWCL_NONE.
//              If both color and bitonal are enabled on a side when
//              AUTOCOLORCONTENT is turned on, CAMERAENABLED will be set to
//              FALSE for the bitonal camera on that side.
#define ICAP_AUTOCOLORCONTENT               0x8093
#define   TWCL_NONE                         0
#define   TWCL_LOW                          1
#define   TWCL_MEDIUM                       2
#define   TWCL_HIGH                         3
#define   TWCL_CUSTOM                       4


// ICAP_AUTOCOLORTHRESHOLD
// Family:      A2O2, Blaze, Falcon, Inferno, Panther, Phoenix, Piranha, 
//              Piranha2, Wildfire
// Type:        TWTY_INT32
// Container:   Range (per camera)
// Allowed:     0 to 100
// Default:     Falcon, Panther, Piranha2: 20
//              Other: 50
// Notes:       The color threshold or intensity (i.e. pale blue vs. dark blue)
//              at which a given a color will be included in the Color Amount
//              calculation . A higher value indicates that a more intense color
//              is required.
#define ICAP_AUTOCOLORTHRESHOLD             0x8094


// ICAP_BACKGROUNDADJUSTAGGRESSIVENESS
// Family:      A2O2, Blaze, Falcon, Panther, Piranha, Piranha2
// Type:        TWTY_INT32
// Container:   Range (per color camera)
// Allowed:     -10 to 10
// Default:     0
// Notes:       The background color adjustment aggressiveness
#define ICAP_BACKGROUNDADJUSTAGGRESSIVENESS    0x80B0


// ICAP_BACKGROUNDADJUSTAPPLYTO
// Family:      A2O2, Blaze, Falcon, Panther, Piranha, Piranha2
// Type:        TWTY_UINT16
// Container:   Enumeration (per color camera)
// Allowed:     TWBA_ALL,TWBA_NEUTRAL,TWBA_PREDOMINATE
// Default:     TWBA_PREDOMINATE
// Notes:       The background color adjustment apply to 
#define ICAP_BACKGROUNDADJUSTAPPLYTO        0x80AF
#define   TWBA_ALL                          0
#define   TWBA_NEUTRAL                      1
#define   TWBA_PREDOMINATE                  2


// ICAP_BACKGROUNDADJUSTMODE
// Family:      A2O2, Blaze, Falcon, Panther, Piranha, Piranha2
// Type:        TWTY_UINT16
// Container:   Enumeration (per color camera)
// Allowed:     TWBS_NONE,TWBS_AUTO,TWBS_CHANGETOWHITE, TWBS_AUTOMATICBASIC
// Default:     TWCL_NONE
// Notes:       The background smoothing mode
#define ICAP_BACKGROUNDADJUSTMODE           0x80AE
#define   TWBS_NONE                         0
#define   TWBS_AUTOMATIC                    1
#define   TWBS_CHANGETOWHITE                2
#define   TWBS_AUTOMATICBASIC               3


// ICAP_COLORBALANCEAUTOMATICAGGRESSIVENESS
// Family:      Falcon, Piranha2, Rufous
// Type:        TWTY_INT32
// Container:   Range (per color camera)
// Allowed:     -2 to 2
// Default:     0
// Notes:       Indicates how aggressive the automatic white balance will be.
//              This is only available when ICAP_COLORBALANCEMODE set to 
//              TWCBM_AUTOMATICADVANCED
#define ICAP_COLORBALANCEAUTOMATICAGGRESSIVENESS   0x810A


// ICAP_COLORBALANCEMODE
// Family:      Blaze, Falcon, Piranha2, Rufous
// Type:        TWTY_UINT16
// Container:   Enumeration (per color camera)
// Allowed:     Blaze:  TWCBM_NONE, TWCBM_MANUAL
//              Others: TWCBM_NONE, TWCBM_MANUAL, TWCBM_AUTOMATIC, 
//                      TWCBM_AUTOMATICBASIC 
// Default:     Blaze:  TWCBM_NONE
//              Others: TWCBM_AUTOMATICBASIC
// Notes:       Allows the user to select the method for adjusting the color 
//              balance of a color image.
//              TWCBM_NONE means no adjustment is made.
//              TWCBM_MANUAL means the user can adjust Red, Green and Blue  
//              (see ICAP_COLORBALANCEREAD/GREEN/BLUE).
//              TWCBM_AUTOMATICBASIC means the scanner will automatically  
//              adjust the balance to be white.
//              TWCBM_AUTOMATIC is the same as TWCBM_AUTOMATICBASIC but 
//              the user can also adjust the aggressiveness of the balance 
//              (see ICAP_COLORBALANCEAUTOMATICAGRESSIVENESS).
#define ICAP_COLORBALANCEMODE               0x8109
#define    TWCBM_NONE                       0
#define    TWCBM_MANUAL                     1
#define    TWCBM_AUTOMATICBASIC             2
#define    TWCBM_AUTOMATIC                  3


// ICAP_COLORBALANCEBLUE
// ICAP_COLORBALANCEGREEN
// ICAP_COLORBALANCERED
// Family:      A2O2, Blaze, Falcon, Fosters, Panther, Piranha, Piranha2, Rufous
// Type:        TWTY_INT32
// Container:   Range (per camera)
// Allowed:     -50 to 50 step 1 for the GUI
//		        -1000 to 1000 step 20 for programmatic
// Default:     0
// Notes:       Allow the user to adjust the color balance. This is ignored 
//              if ICAP_COLORBALANCEMODE is not set to TWCBM_MANUAL.
#define ICAP_COLORBALANCEBLUE               0x80B1
#define ICAP_COLORBALANCEGREEN              0x80B2
#define ICAP_COLORBALANCERED                0x80B3


// CAP_COLORBRIGHTNESSMODE
// Family:      Blaze, Falcon, Piranha2
// Type:        TWTY_UINT16
// Container:   Enumeration (per color camera)
// Allowed:     Blaze:  TWCBR_NONE, TWCBR_MANUAL
//              Others: TWCBR_NONE, TWCBR_MANUAL, TWCBR_AUTOMATIC
// Default:     Blaze:  TWCBR_NONE
//              Others: TWCBR_AUTOMATIC
// Notes:       Allows the user to select the method for adjusting the  
//              brightness and contrast of a color or grayscale image.
//              TWCBR_NONE means no adjustment is made.
//              TWCBR_MANUAL means the user can adjust Brightness and  
//              Contrast (see ICAP_BRIGHTNESS and ICAP_CONTRAST).
//              TWCBR_AUTOMATICBASIC means the scanner will  
//              automatically adjust the image.
#define ICAP_COLORBRIGHTNESSMODE            0x8108
#define    TWCBR_NONE                       0
#define    TWCBR_MANUAL                     1
#define    TWCBR_AUTOMATICBASIC             2


// ICAP_COLORSHARPEN
// Family:      Blaze, Falcon, Panther, Piranha2, Rufous
// Type:        TWTY_UINT32
// Container:   Range  (per output stream)
// Allowed:     0 - 3
// Default:     1
// Notes:       Apply sharpening to image - 0 indicates no sharpening.
//				1 is normal, 2 is more sharpening, and  a value of 3
//				applies a lot of sharpening.
#define ICAP_COLORSHARPEN					0x8102


// ICAP_COLORSHARPENING
// Family:      Prism, Wildfire, Viper
// Type:        TWTY_INT16
// Container:   Enumeration (per camera)
// Allowed:     TWCS_NONE, TWCS_3X3FIRFILTER
// Default:     TWCS_3X3FIRFILTER
// Notes:       Color image sharpening.  This setting only has
//              meaning if ICAP_COMPRESSION is set to TWCP_JPEG.
#define ICAP_COLORSHARPENING                0x8041
#define   TWCS_NONE                         0
#define   TWCS_3X3FIRFILTER                 1


// ICAP_COLORTABLE
// Family:      A2O2, Alf, Alien, Fosters, Inferno, Mustang2, Phoenix, 
//              Piranha, Pony, Prism, Wildfire, Viper
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     (as discovered in scanner)
// Notes:       Selects the color table to use.  This capability
//              is like ICAP_HALFTONES in that there is no
//              programmatic way to guarantee the results of a
//              given selection.  The best tactic is to present
//              the data to the user 'as is' and let them select
//              what they want.
#define ICAP_COLORTABLE                     0x8020
#define    TWCT_UNKNOWN                     -1
#define    TWCT_DEFAULT                     0
#define    TWCT_FILE01                      1
#define    TWCT_FILE02                      2
#define    TWCT_FILE03                      3
#define    TWCT_FILE04                      4
#define    TWCT_FILE05                      5
#define    TWCT_FILE06                      6
#define    TWCT_FILE07                      7
#define    TWCT_FILE08                      8
#define    TWCT_FILE09                      9
#define    TWCT_FILE10                      10
#define    TWCT_FILE11                      11
#define    TWCT_FILE12                      12
#define    TWCT_FILE13                      13
#define    TWCT_FILE14                      14
#define    TWCT_FILE15                      15
#define    TWCT_FILE16                      16
#define    TWCT_FILE17                      17
#define    TWCT_FILE18                      18
#define    TWCT_FILE19                      19
#define    TWCT_FILE20                      20
#define    TWCT_FILE21                      21
#define    TWCT_FILE22                      22
#define    TWCT_FILE23                      23
#define    TWCT_FILE24                      24
#define    TWCT_FILE25                      25
#define    TWCT_FILE26                      26
#define    TWCT_FILE27                      27
#define    TWCT_FILE28                      28
#define    TWCT_FILE29                      29
#define    TWCT_FILE30                      30
#define    TWCT_FILE31                      31
#define    TWCT_FILE32                      32
#define    TWCT_FILE33                      33
#define    TWCT_FILE34                      34
#define    TWCT_FILE35                      35
#define    TWCT_FILE36                      36
#define    TWCT_FILE37                      37
#define    TWCT_FILE38                      38
#define    TWCT_FILE39                      39
#define    TWCT_FILE40                      40
#define    TWCT_FILE41                      41
#define    TWCT_FILE42                      42
#define    TWCT_FILE43                      43
#define    TWCT_FILE44                      44
#define    TWCT_FILE45                      45
#define    TWCT_FILE46                      46
#define    TWCT_FILE47                      47
#define    TWCT_FILE48                      48
#define    TWCT_FILE49                      49
#define    TWCT_FILE50                      50
#define    TWCT_FILE51                      51
#define    TWCT_FILE52                      52
#define    TWCT_FILE53                      53
#define    TWCT_FILE54                      54
#define    TWCT_FILE55                      55
#define    TWCT_FILE56                      56
#define    TWCT_FILE57                      57
#define    TWCT_FILE58                      58
#define    TWCT_FILE59                      59
#define    TWCT_FILE60                      60

// ICAP_CROPPINGMODE
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno,  
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, 
//              Prism, Rufous, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     See below...
// Default:     TWCR_AUTOMATICBORDERDETECTION
// Notes:       Selects the mode of cropping.  This capability is
//              tied to ICAP_AUTOMATICBORDERDETECTION, where FALSE
//              equals TWCR_TRANSPORT and TRUE equals
//              TWCR_AUTOMATICBORDERDETECTION.
//              Continuous cropping mode is also known as Long Paper mode.    
//              If the user set one of the camera to TWCR_CONTIUOUS
//              all the camera force to set cropping to TWCR_CONTIUOUS
//              and if all camera in TWCR_CONTIUOUS cropping mode, the 
//              user change one of the camera to other cropping mode
//              then all the camera will force to set the same cropping mode
#define ICAP_CROPPINGMODE                   0x8022
#define    TWCR_AUTOMATICBORDERDETECTION    0
#define    TWCR_TRANSPORT                   1
#define    TWCR_DOCUMENT                    2
#define    TWCR_AGGRESSIVEAUTOCROP          3
#define    TWCR_CONTINUOUS                  4
#define    TWCR_MULTIPLEAGGRESSIVE          5
#define    TWCR_PHOTO                       6
#define    TWCR_PHOTOINROI                  7


// ICAP_DOCUMENTTYPE
// Family:      A2O2, Blaze, Falcon, Fosters, Oasis, Panther, Piranha, Piranha2, Rufous
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     See below
// Default:     TWDT_TEXTWITHGRAPHIC
//		        Oasis: TWDT_PHOTO
// Notes:       Allows user to select the type of document being scanned.
#define ICAP_DOCUMENTTYPE                   0x80AC
#define    TWDT_PHOTO                       0
#define    TWDT_TEXTWITHGRAPHICS            1
#define    TWDT_TEXTWITHPHOTO               2
#define    TWDT_TEXT                        3


// ICAP_ECDO
// Family:      A2O2, Blaze, Falcon, Fosters, Panther, Piranha, Piranha2, Rufous
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     None
// Notes:       Allows user to select the ECDO color for bitonal/gray camera.
//              Also refer to DAT_ECDO later in this file.
#define ICAP_ECDO		                    0x80B8
#define    TWCD_UNKNOWN                     -1
#define    TWCD_NONE	                    0
#define    TWCD_FILE01                      1
#define    TWCD_FILE02                      2
#define    TWCD_FILE03                      3
#define    TWCD_FILE04                      4
#define    TWCD_FILE05                      5
#define    TWCD_FILE06                      6
#define    TWCD_FILE07                      7
#define    TWCD_FILE08                      8
#define    TWCD_FILE09                      9
#define    TWCD_FILE10                      10
#define    TWCD_FILE11                      11
#define    TWCD_FILE12                      12
#define    TWCD_FILE13                      13
#define    TWCD_FILE14                      14
#define    TWCD_FILE15                      15
#define    TWCD_FILE16                      16
#define    TWCD_FILE17                      17
#define    TWCD_FILE18                      18
#define    TWCD_FILE19                      19
#define    TWCD_FILE20                      20
#define    TWCD_FILE21                      21
#define    TWCD_FILE22                      22
#define    TWCD_FILE23                      23
#define    TWCD_FILE24                      24
#define    TWCD_FILE25                      25
#define    TWCD_FILE26                      26
#define    TWCD_FILE27                      27
#define    TWCD_FILE28                      28
#define    TWCD_FILE29                      29
#define    TWCD_FILE30                      30
#define    TWCD_FILE31                      31
#define    TWCD_FILE32                      32
#define    TWCD_FILE33                      33
#define    TWCD_FILE34                      34
#define    TWCD_FILE35                      35
#define    TWCD_FILE36                      36
#define    TWCD_FILE37                      37
#define    TWCD_FILE38                      38
#define    TWCD_FILE39                      39
#define    TWCD_FILE40                      40
#define    TWCD_FILE41                      41
#define    TWCD_FILE42                      42
#define    TWCD_FILE43                      43
#define    TWCD_FILE44                      44
#define    TWCD_FILE45                      45
#define    TWCD_FILE46                      46
#define    TWCD_FILE47                      47
#define    TWCD_FILE48                      48
#define    TWCD_FILE49                      49
#define    TWCD_FILE50                      50
#define    TWCD_FILE51                      51
#define    TWCD_FILE52                      52
#define    TWCD_FILE53                      53
#define    TWCD_FILE54                      54
#define    TWCD_FILE55                      55
#define    TWCD_FILE56                      56
#define    TWCD_FILE57                      57
#define    TWCD_FILE58                      58
#define    TWCD_FILE59                      59
#define    TWCD_FILE60                      60


// ICAP_ECDOAGGRESSIVENESS
// Family:      Blaze, Falcon, Panther, Piranha2
// Type:        TWTY_INT32
// Container:   Range (per output stream, bitonal only)
// Allowed:     -10 to 10
// Default:     0
// Notes:       Change the amount of multi-color dropout applied to
//				a bitonal image.
#define ICAP_ECDOAGGRESSIVENESS				0x8103


// ICAP_ECDOTREATASCOLOR
// Family:      A2O2
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Allows you to indicate that a color other than black or
//              dark blue ink was used for the data entered on the form.
//              This is not available when ECDO is set to (none).
//              This feature is not available for all models.
#define ICAP_ECDOTREATASCOLOR              0x80C3


// ICAP_FILTERBACKGROUND
// Family:      A2O2, Alf, Alien, Fosters, Inferno, Mustang2, Phoenix,
//              Piranha, Pony, Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Range
// Allowed:     0 - 255
// Default:     245
// Notes:       Selects the grayscale color to replace the dropout
//              color when capturing bitonal images...
#define ICAP_FILTERBACKGROUND               0x8026


// ICAP_FILTERENUM
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, 
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, 
//              Prism, Rufous, Wildfire
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     See ICAP_FILTER
// Default:     TWFT_NONE
// Notes:       Tied to ICAP_FILTER...
//              ICAP_FILTER is an array...and we need an enum (sigh)...
#define ICAP_FILTERENUM                     0x8024


// ICAP_FILTERPROCESSING
// Family:      Phoenix
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     TWFP_NORMAL, TWFP_LOWRES
// Default:     TWFP_NORMAL
// Notes:       If color dropout at low resolutions results in
//              unwanted artifacts, try changing this capability
//              to TWFP_LOWRES.
#define ICAP_FILTERPROCESSING               0x8088
#define      TWFP_NORMAL                         0
#define      TWFP_LOWRES                         1


// ICAP_FILTERTHRESHOLD
// Family:      A2O2, Alf, Alien, Fosters, Inferno, Mustang2, Phoenix,
//              Piranha, Pony, Prism, Wildfire
// Type:        TWTY_UINT16
// Container:   Range
// Allowed:     0 - 255
// Default:     175
// Notes:       Selects the threshold point of the color data when
//              capturing bitonal images...
#define ICAP_FILTERTHRESHOLD                0x8025


// ICAP_FLIPBACKGROUNDCOLOR
// Family:      Gemini
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       5000/7000 only.  This is Border Reduction.  A feature
//              that changes the borders on the left and the right to
//              white the opposite pixel flavor 'color'.  A value of
//              TRUE does the change, a value of FALSE leaves it as is...
#define ICAP_FLIPBACKGROUNDCOLOR            0x8043


// ICAP_FORCECOMPRESSION
// Family:      Gemini, Inferno, Phoenix, Prism, Wildfire, Viper
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     same rules as ICAP_COMPRESSION
// Default:     TWCP_NONE
// Notes:       A throughput efficiency mechanism.  If an application
//              requires uncompressed data, but the size of the image
//              is more than SCSI can manage, then use this capability
//              to force the scanner to compress the data, and the
//              Source to decompress in software before transferring
//              it to the application.  Note that with the Prism
//              this value is always TWCP_JPEG for the color cameras.
#define ICAP_FORCECOMPRESSION               0x8008


// ICAP_FOREGROUNDBOLDNESSAGGRESSIVENESS
// Family:      Falcon
// Type:        TWTY_INT32
// Container:   Range (per color camera)
// Allowed:     -10 to 10
// Default:     0
// Notes:       The foreground boldness aggressiveness setting.
//              This setting only has meaning if
//              ICAP_FOREGROUNDBOLDNESSMODE is TWFB_AUTOMATIC.
#define ICAP_FOREGROUNDBOLDNESSAGGRESSIVENESS    0x810E


// ICAP_FOREGROUNDBOLDNESSMODE
// Family:      Falcon
// Type:        TWTY_UINT16
// Container:   Enumeration (per color camera)
// Allowed:     TWFB_NONE, TWFB_AUTOMATICBASIC, TWFB_AUTOMATIC
// Default:     TWFB_NONE
// Notes:       The foreground boldness mode
#define ICAP_FOREGROUNDBOLDNESSMODE         0x810F
#define   TWFB_NONE                         0
#define   TWFB_AUTOMATICBASIC               1
#define   TWFB_AUTOMATIC                    2


// ICAP_FRAMELENGTHCONTROL
// Family:      Gemini
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       5000/7000 only.  Controls the way the scanner detects
//              the end of the document.  A value of TRUE reflects the
//              way all other scanners work.  A value of FALSE should
//              be used for documents with ragged bottom edges...
#define ICAP_FRAMELENGTHCONTROL             0x8042


// ICAP_FRAMESANGLE
// Family:      Blaze, Falcon, Oasis, Panther, Piranha2
// Type:        TWTY_FRAMEANGLE
// Container:   Enumeration
// Allowed:     0,0 - ICAP_PHYSICALWIDTH,ICAP_PHYSICALHEIGHT
// Default:     value of ICAP_SUPPORTEDSIZE
// Notes:       With the support of angles it becomes necessary to
//              provide a way to specify the coordinates and the
//              angle simultaneously, otherwise it's impossible to
//              provide proper validation.  So if you are working
//              with non-zero angles, then use this capability
//              instead of ICAP_FRAMES or DAT_IMAGELAYOUT.
#define ICAP_FRAMESANGLE                    0x80BB


// ICAP_GAMMAENABLED
// Family:      Blaze, Falcon, Inferno, Panther, Phoenix, Piranha2, Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       A value of TRUE causes the scanner to apply
//              its gamma table to the image.  This only has
//              meaning for grayscale output.
#define ICAP_GAMMAENABLED                   0x8074


// ICAP_GRAYSCALE
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, 
//              Mustang2, Panther, Phoenix, Piranha, Piranha2, Pony, 
//              Prism,, Rufous, Wildfire
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       If set to TRUE then the color output will be
//              converted to grayscale.  You can also get to
//              this mode by setting ICAP_PIXELTYPE to
//              TWPT_GRAY, though if you do it that way you
//              will get grayscale output for both the front
//              and the rear images...
#define ICAP_GRAYSCALE                      0x802E


// ICAP_HALFTONESQUALITY
// Family:      Viper (3590/4500)
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     TWHQ_SOFTWAREFAST and TWHQ_SOFTWAREBEST or
//              TWHQ_HARDWAREBEST
// Default:     TWHQ_SOFTWAREFAST or TWHQ_HARDWAREBEST
// Notes:       Selects the quality/performance of the 
//              ChromaTHR(tm) software quality settings for
//              making bitonal images from color data.  If
//              a hardware card is present, than it is the
//              only option available; otherwise the two
//              software options are made available...
#define ICAP_HALFTONESQUALITY               0x801F
#define    TWHQ_NONE                        0
#define    TWHQ_SOFTWAREFAST                1
#define    TWHQ_SOFTWAREBEST                2
#define    TWHQ_HARDWAREBEST                3


// ICAP_HOLEFILLENABLED
// Family:      Falcon, Piranha, Piranha2
// Type:        TWTY_BOOL
// Container:   OneValue (per output stream)
// Allowed:     TRUE / FALSE
// Default:     FALSE
// Notes:       Hole Fill enabled/disabled.
#define ICAP_HOLEFILLENABLED           	    0x8104


// ICAP_IMAGEEDGEFILL
// Family:      A2O2, Falcon, Fosters, Mustang2, Panther, Piranha, 
//              Piranha2, Pony, Rufous
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     (see list below)
// Default:     TWIE_AUTOMATIC if the scanner is supported. 
//              Otherwise TWIE_NONE is the default
// Notes:       Fills in each edge of the image with the selected color. 
//              The TWIE_AUTOMATIC and TWIE_AUTOMATICWITHTEAR are not available
//              for all scanners.
#define ICAP_IMAGEEDGEFILL                  0x8095
#define      TWIE_NONE                      0
#define      TWIE_WHITE                     1
#define      TWIE_BLACK                     2
#define      TWIE_AUTOMATIC                 3
#define      TWIE_AUTOMATICWITHTEAR         4


// ICAP_IMAGEEDGEFILLALLSIDES
// Family:      A2O2, Falcon, Fosters, Panther, Piranha, Piranha2, Rufous
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       Indicates if all of the sides for image edge fill are the same
//              value, based on ICAP_IMAGEEDGETOP
#define ICAP_IMAGEEDGEFILLALLSIDES          0x80B9
#define ICAP_IMAGEEDGEFILLALLSIZE           0x80B9	// Deprecated TWAIN 10.x+	


// ICAP_IMAGEEDGE*
// Family:      A2O2, Falcon, Fosters, Mustang2, Panther, Piranha, Piranha2,
//              Pony, Rufous
// Type:        TWTY_FIX32
// Container:   Range (per camera)
// Allowed:     0 to ICAP_PHYSICALWIDTH for left and right
//              0 to ICAP_PHYSICALHEIGHT for the the top and bottom
// Default:     0
// Notes:       Amount of fill for the left/right/top/bottom edge of the image
#define ICAP_IMAGEEDGELEFT                  0x8096
#define ICAP_IMAGEEDGERIGHT                 0x8097
#define ICAP_IMAGEEDGETOP                   0x8098
#define ICAP_IMAGEEDGEBOTTOM                0x8099


// ICAP_IMAGEFILEFORMAT
// Family:      (all)
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     TWFF_BMP, TWFF_JFIF, TWFF_TIFF, TWFF_RAW...
// Default:     TWFF_BMP
// Notes:       This is a standard TWAIN capability, we're just
//              adding a custom argument to it
#define TWFF_RAW                            0x8001
#define TWFF_RAWBMP                         0x8002


// CAP_IMAGEMAGNIFICATIONFACTOR
// Family:      n/a
// Type:        TWTY_UINT16
// Container:   Range
// Allowed:     0 - 255
// Default:     0
// Notes:       Helps the scanner determine the 'real' size of images
//              coming from the P-Card.  This information is used to
//              optimize buffer thresholding.
//              This capability is not available at this time.
#define CAP_IMAGEMAGNIFICATIONFACTOR        0x806B


// ICAP_LAMPSAVER
// Family:      Prism, Viper
// Type:        TWTY_BOOL
// Container:   OneValue
// Allowed:     TRUE / FALSE
// Default:     TRUE
// Notes:       If TRUE the scanner's lamps will automatically turn off
//              after ten minutes of non-use; saving power and extending
//              the life of the lamps.
#define ICAP_LAMPSAVER                      0x8005


// ICAP_LAMPTIMEOUT
// Family:      Prism, Viper
// Type:        TWTY_UINT16
// Container:   Range
// Allowed:     Viper:0,10  Prism:0-30 (minutes)
// Default:     10, 20
// Notes:       Controls the lamp saver option.  Please note that this
//              capability should be used instead of ICAP_LAMPSAVER.
#define ICAP_LAMPTIMEOUT                    0x802A


// ICAP_MEDIATYPE
// Family:      A2O2, Blaze, Falcon, Fosters, Panther, Piranha, Piranha2, Rufous
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     See below
// Default:     TWMT_PLAINPAPER
// Notes:       Allows user to select type of media being scanned.
//              The front and rear camera setting must be the same.
#define ICAP_MEDIATYPE                      0x80B6
#define		TWMT_CARDSTOCK                  0
#define		TWMT_GLOSSYPAPER                1
#define		TWMT_MAGAZINE                   2
#define		TWMT_PLAINPAPER                 3
#define		TWMT_THINPAPER                  4


// ICAP_ORTHOGONALROTATE
// Family:      Blaze, Falcon, Piranha2
// Type:        TWTY_UINT16
// Container:   Enumeration (per camera)
// Allowed:     (see below)
// Default:     TWOROT_NONE
// Notes:       Orthogonal rotation
#define ICAP_ORTHOGONALROTATE               0x80D5
#define    TWOROT_NONE                      0
#define    TWOROT_AUTOMATIC                 1
#define    TWOROT_90                        2
#define    TWOROT_180                       3
#define    TWOROT_270                       4
#define    TWOROT_AUTOMATIC_90              5
#define    TWOROT_AUTOMATIC_180             6
#define    TWOROT_AUTOMATIC_270             7


// ICAP_OVERSCANX
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, Panther, 
//              Phoenix, Piranha, Piranha2, Prism, Rufous, Wildfire, Viper
// Type:        TWTY_FIX32
// Container:   Range (per camera)
// Allowed:     3500: 0-0.5"  All others: 0-0.375"
// Default:     3500: 0.5"    All others: 0.375"
// Notes:       Amount of overscan (in ICAP_UNITS) on the left and
//              right sides of the cropping region.
#define ICAP_OVERSCANX                      0x8006


// ICAP_OVERSCANY
// Family:      A2O2, Alf, Alien, Blaze, Falcon, Fosters, Inferno, Panther, 
//              Phoenix, Piranha, Piranha2, Prism, Rufous, Wildfire, Viper
// Type:        TWTY_FIX32
// Container:   Range (per camera)
// Allowed:     3500: 0-0.5"  All others: 0-0.375"
// Default:     3500: 0.5"    All others: 0.375"
// Notes:       Amount of overscan (in ICAP_UNITS) on the top and
//              bottom sides of the cropping region.
#define ICAP_OVERSCANY                      0x8007


// ICAP_PADDING
// Family:      n/a
// Type:        TWTY_UINT16
// Container:   Enumeration
// Allowed:     (see list below)
// Default:     TWPG_AUTO
// Notes:       Selects the padding to force the bytes-per-row
//              (or the stride) to always land on a specific
//              boundry...
#define ICAP_PADDING                        0x80AA
#define   TWPG_AUTO                         0
#define   TWPG_NONE                         1
#define   TWPG_BYTE                         2
#define   TWPG_WORD                         3
#define   TWPG_LONGWORD                     4
#define   TWPG_OCTAWORD                     5


// ICAP_PHYSICALHEIGHTADJUST
// Family:      Blaze, Falcon, Panther, Piranha2
// Type:        TWTY_FIX32
// Container:   Range
// Allowed:     Scanner specific
// Default:     Scanner specific
// Notes:       Adjusts the value of ICAP_PHYSICALHEIGHT.  Applications must
//              set this value to have access to the full scan length of the
//              ADF.
#define ICAP_PHYSICALHEIGHTADJUST           0x8101


// ICAP_SKEWANGLE
// Family:      A2O2, Blaze, Falcon, Fosters, Panther, Piranha, Piranha2, Rufous
// Type:        TWTY_UINT16
// Container:   Range
// Allowed:     -360000 - 360000
// Default:     0
// Notes:       Selects the deskew angle for fixed cropping.
#define ICAP_SKEWANGLE                      0x80B5


// ICAP_STREAKREMOVALAGGRESSIVENESS
// Family:      Blaze, Falcon, Panther, Piranha, Piranha2
// Type:        TWTY_INT32
// Container:   Range
// Allowed:     -2 to 2
// Default:     0
// Notes:       Streak removal aggressiveness level. -2 is least aggressive
//              while 2 is most aggressive. The aggressiveness level is
//              meaningless if streak removal ‘enabled’ is FALSE.
#define ICAP_STREAKREMOVALAGGRESSIVENESS     0x80C7


// ICAP_STREAKREMOVALENABLED
// Family:      Blaze, Falcon, Panther, Piranha, Piranha2
// Type:        TWTY_BOOL
// Container:   OneValue (per output stream)
// Allowed:     TRUE / FALSE
// Default:     Piranha: FALSE
//              Others: TRUE
// Notes:       Streak Removal enabled/disabled. If TRUE, then the
//              streak removal ‘aggressiveness level’ has meaning.
#define ICAP_STREAKREMOVALENABLED           0x80C6


// ICAP_VERTICALBLACKLINEREMOVAL
// Family:      Prism (not implemented, ignore this for now)
// Type:        TWTY_BOOL
// Container:   OneValue (per camera)
// Allowed:     TRUE/FALSE
// Default:     TRUE
// Notes:       Controls vertical black line removal (VBLR).
//              Only supported for bitonal with ATP...
#define ICAP_VERTICALBLACKLINEREMOVAL       0x8021



////////////////////////////////////////////////////////////////////////////////
//                      DAT Section
////////////////////////////////////////////////////////////////////////////////



//
//  A2O2/Falcon/Inferno/Phoenix/Piranha/Piranha2/Wildfire auto color learning
//  (DG_CONTROL)...
//  Notes:  This utility will determine the proper ‘Color Amount’ value
//          to use with the current ‘Color Threshold’ value, in order to
//          save a representative stack of color documents as either color
//          or grayscale images...
#define DAT_AUTOCOLORLEARN                  0x8006


//
//  Color Table Map (DG_CONTROL)...
//  Notes:  This data structure allows an application to
//          map the ICAP_COLORTABLE numeric values to
//          both the English and current language strings.
//          The call allocates the data structure.  The
//          application parses the items (a value of
//          0xFFFF for ColorTableValue indicates the end
//          of the array).
#define DAT_COLORTABLEMAP                   0x8003
typedef struct
{
    TW_UINT16    ColorTableValue;
    TW_STR255    szEnglish;
    TW_STR255    szCurrent;
} TW_COLORTABLEMAP, *pTW_COLORTABLEMAP;


//
//  Device Information
//  Notes:  This data structure provides additional
//          information about the device that may be
//          useful with some system message, such
//          as WM_DEVICECHANGE.  The dwFields value
//          indicates how many fields are supported
//          by the driver (the value doesn't include
//          the dwFields, itself)...
//
//          dwModelNumber  - ex: 200, 990, 3520...
//          szModelName    - ex: i200, 990...
//          szProtocol     - ex: ASPI, SCSISCAN...
//          dwFirmware     - ex: 10427 (1.4.27)
//
//          Only if szProtocol is SCSISCAN
//          szDeviceName   - \\.\Scanner# style
//          szDeviceChange - \\?\SBP2... style
//
//          Only if szProtocol is ASPI
//          dwHostAdapter
//          dwTarget
//
#define DAT_DEVICEINFO                      0x8005
#define TWCNST_DEVICEINFOFIELDS             9
typedef struct
{
    TW_UINT32    dwFields;
    TW_UINT32    dwModelNumber;             // field 1
    TW_STR255    szModelName;               // field 2
    TW_STR255    szProtocol;                // field 3
    TW_STR255    szDeviceName;              // field 4
    TW_UINT32    dwHostAdapter;             // field 5
    TW_UINT32    dwTarget;                  // field 6
    // put new fields here...
    TW_STR255    szDeviceChange;            // field 7  + 256 =  256
    TW_UINT32    dwFirmware;                // field 8  +  32 =  288
	TW_STR255	 szDriverFilename;			// field 9  + 256 =  544
    // always reduce this if we add fields...
    TW_UINT8     reserved[65536-544];
} TW_DEVICEINFO, *pTW_DEVICEINFO;


//  Driver Electronic Color Dropout (for drivers that support ICAP_ECDO)
//
//  Notes:  Use this structure to explore and manage dropout colors.
//
//          This data structure allows an application to map the ICAP_ECDO
//          numeric values to both the English and current language strings.
//          ICAP_ECDO is used to select a dropout color for the current camera.
//
//          The driver will allocate the returned data structure.
//
//          To get the list of dropout choices (for when an application wants
//          to present the list on its own user interface):
//          -----------------------------------------------------------------
//          1. Application calls DG_CONTROL / DAT_ECDO / MSG_GET to get
//             the list of all the choices.
//          2. Driver returns the list of choices (i.e. fills in the
//             TW_ECDO structure for each choice).
//          3. Application loops through the returned list until both szEnglish
//             and szCurrent are blank (which indicates the end of the list).
//
//          To find a dropout choice (for when an application needs to find
//          the TWCD_* for use with ICAP_ECDO):
//          -----------------------------------------------------------------
//          1. Application calls DG_CONTROL / DAT_ECDO / MSG_GET to get
//             the list of all the choices.
//          2. Driver returns the list of choices (i.e. fills in the
//             TW_ECDO structure for each choice).
//          3. Application loops through the returned list until it finds the
//             desired English dropout name (use szEnglish because this will
//             always be consistent no matter what the language of the driver
//             is; where szCurrrent will be based on the driver's current
//             language, which may be different then the last time the list
//             was returned).
//          4. Application uses the EcdoValue of the desired dropout choice
//             as the setting for ICAP_ECDO. This means the EcdoValue
//             will be one of the TWCD_* defines (e.g. TWCD_FILE01).
//
#define DAT_ECDO		                   0x800A
typedef struct
{
    TW_UINT16    EcdoValue;
    TW_STR255    szEcdo;
    TW_STR255    szText;
    TW_UINT8     Reserved[1024];            // Room for more stuff
} TW_ECDO, *pTW_ECDO;


// Driver Logs (DG_CONTROL / DAT_LOG / MSG_GET)
//
// This will get a log and save it in the provided file. There are text based logs and XML based
// logs. Some logs are scanner specific, so the content will not be common among all scanner
// families, and some are common so the content can be common among all scanners.
//
// Text based logs are ones that can be easily viewed in something like Notepad, each log entry
// will be on its own line. These logs contain the same information as is shown on the driver UI
// on the Log tab. The language for the translated text will be based on the current language
// selected for the driver.
//
// The XML based logs will contain data in a XML format. For details on the XML format for
// a log, refer to the TWAIN_Features.htm in the Intergrator’s Guide.
//
// For devices that support the Flatbed as an accessory (e.g. i1300, i1400), the application
// will need to make a second call to get the flatbed’s information (see DeviceType below). This
// is necessary because the flatbed is a separate device.
//
// To generate an EKLOG specify a LogType of TWGL_EKLOG.  The DeviceType is ignored.  The
// Filename specifies the full path and file for the log, and it should end in .eklog.
// The Description is stored with the EKLOG.  Use SaveImages to specify if .tif and .jpg
// files should be included (including them may result in a very large EKLOG file).
//
#define DAT_LOG						0x800C
typedef struct {
	TW_UINT16	LogType;			// log to get (see TWGL_*)
	TW_UINT16	DeviceType;			// ADF or Flatbed (see TWDV_*)
	TW_STR255	Filename;			// full path and name to store log into
	TW_STR255	Description;		// Reason for log (use with TWLG_EKLOG)
	TW_BOOL		SaveImages;			// Save images (use with TWLG_EKLOG)
	TW_UINT8	Reserved[254];		// Room for future expansion
} TW_LOG, *pTW_LOG;

// different logs that can be retrieved via DAT_LOG. The comment indicate whether the
// log is text based or XML base, and whether it is common among all scanner families or not.
#define    TWGL_GENERAL					0	// text, not common
#define    TWGL_OPERATOR				1	// text, not common
#define    TWGL_GENERAL_XML				2	// XML, common
#define    TWGL_EKLOG					3	// EKLOG (TWDV_* value is ignored)


// different devices, which can be used by multiple CAPs/ICAPs/DATs
#define    TWDV_UNKNOWN					0	// unknown/reserved
#define    TWDV_ADF						1	// base scanner
#define    TWDV_FLATBED					2	// flatbed scanner


// OCP buttons (DG_CONTROL / DAT_OCPBUTTONS / MSG_SET)
//
// Scanner supported: Falcon, Piranha2
// Max number of buttons: 9
//
// This feature allows the application to set the displayed text on the OCP for
// each button. It also allows the application to set the Paper Source for each
// button. The driver may not remember this information. So we recommand
// that the application should send the text after successfully MSG_OPENDS.
//
// To configure the number of buttons for the user to scroll through, the
// App must send an array of TW_OCPBUTTONS structures which has the number
// of the buttons defined plus an extra one that has Text field blank.
// For example, if the user wants to setup the text for 3 buttons then
// define the array of 4 TW_OCPBUTTONS structures. Set the Text for each
// the first 3 items and set the Text to blank for the 4th.
//
// For the scanners that don’t have the ability to display the text, the
// application still can configure the number of buttons for the user
// to scroll through.
//
// The order of the buttons displayed on the OCP is the same order of the
// buttons defined in the array.
//
// The Text is in the current language. The Ansi code page which is based
// on the CAP_LANGUAGE is used for the conversion.
//
// The Paper Source value to pass in is the TWAIN enum value for the Paper
// Source associated with the button. For eaxmple, TWPU_ADF is the enum value
// associated with the "adf" paper source. For the complete list of Paper
// Source enums, look at the TWPU_* definitions under CAP_PAPERSOURCE.
//
// The NumDataFields value refers to the number of data fields to follow.
// If the PaperSource field is being set, then NumDataFields must be set to 1.
// In the future, we could add other data fields, in which case NumDataFields
// would be incremented by the caller.
//
// The Reserved field must be initialized to zero.
//
// For example, if setting a button to "Color PDF" and the paper source is
// "Document Feeder", then:
//		Text = "Color PDF"
//		NumDataFields = 1
//		PaperSource = TWPU_ADF
//
#define DAT_OCPBUTTONS					0x800E
typedef struct {
	TW_STR255	Text;				// Displayed text on OCP for a button
	TW_UINT32	NumDataFields;		// Number of fields of data that follow this field
	TW_UINT32	PaperSource;		// TWPU_* enum from CAP_PAPERSOURCE
	TW_UINT8	Reserved[504];		// Room for future expansion
} TW_OCPBUTTONS, *pTW_OCPBUTTONS;


//
//  Alien Personality Card (DG_CONTROL)...
//  Notes:  These represent the common commands that pcards
//          may support.  Other commands documented by the
//          manufacturer of the card may be accessed using
//          the DAT_PASSTHRU operation...
#define DAT_PCARD                           0x8002
typedef struct
{
    TW_UINT32    StructSize;
    TW_UINT32    Action;
    TW_UINT32    DataSize;
    TW_MEMREF    Data;
} TW_PCARD, *pTW_PCARD;

// Additional return codes...
#define TWRC_PCARD_CHECKSTATUS              0x8002  // something interesting has happened
#define TWRC_PCARD_FATAL                    0x8003  // the card is dead
#define TWRC_PCARD_RETRYIMAGE               0x8004  // the image failed a quality test
#define TWRC_PCARD_NOCARD                   0x8005  // no card installed

// Supported actions...
#define TWPCD_ACTION_INQUIRY                0
#define TWPCD_ACTION_STATUS                 1
#define TWPCD_ACTION_ATTRIBUTES             2
#define TWPCD_ACTION_DIAGNOSTIC             3

// Support structures for the various actions...
typedef struct
{
    TW_INT8      StructSize[4];             // 0-3
    TW_INT8      VendorId[8];               // 4-11
    TW_INT8      ProductId[16];             // 12-27
    TW_INT8      RevisionNumber[4];         // 28-31
    TW_INT8      BuildNumber[2];            // 32-33
    TW_INT8      Reserved[2048-34];         // 34-2048
} TW_PCARD_INQUIRY, FAR * pTW_PCARD_INQUIRY;

typedef struct
{
    TW_INT8      StructSize[4];             // 0-3
    TW_INT8      Code[4];                   // 4-7
    TW_INT8      Severity;                  // 8
    TW_INT8      Text[1011];                // 9-1019
    TW_INT8      Reserved[2048-1020];       // 1020-2048
} TW_PCARD_STATUS, *pTW_PCARD_STATUS;

typedef struct
{
    TW_INT8      StructSize[4];             // 0-3
} TW_PCARD_ATTRIBUTES, *pTW_PCARD_ATTRIBUTES;

typedef struct
{
    TW_INT8      StructSize[4];             // 0-3
} TW_PCARD_HEADER, *pTW_PCARD_HEADER;


//  Driver Profiles (for drivers that support CAP_PROFILES)
//
//  Notes:  Use this structure to explore and manage profiles. These profiles
//          are compatible with DAT_CUSTOMDSDATA.
//
//          This data structure allows an application to map the CAP_PROFILES
//          numeric values to both the English and current language strings.
//
//          To get the list of profiles (for when an application wants to
//          present the list on its own user interface):
//          -----------------------------------------------------------------
//          1. Application calls DG_CONTROL / DAT_PROFILES / MSG_GET to get
//             the list of all the profiles.
//          2. Driver returns the list of profiles (i.e. fills in the
//             TW_PROFILES structure for each profile)
//          3. Application loops through the returned list until both szEnglish
//             and szName are blank (which indicates the end of the list)
//
//          To find a profile (for when an application needs to find the
//          TWPO_* for use with CAP_PROFILES):
//          -----------------------------------------------------------------
//          1. Application calls DG_CONTROL / DAT_PROFILES / MSG_GET to get
//             the list of all the profiles.
//          2. Driver returns the list of profiles (i.e. fills in the
//             TW_PROFILES structure for each profile)
//          3. Application loops through the returned list until it finds the
//             desired English profile name (use szEnglish because this will
//             always be consistent no matter what the language of the driver
//             is; where szName will be based on the driver's current
//             language, which may be different then the last time the list
//             was returned) OR ID. The application should use szEnglish name
//             if they want to always find a specific profile (i.e. Black and
//             White Doc) OR use ID when it doesn't care about specific names,
//             but wants to use unique IDs to get the same profile even if the
//             user renames it.
//          4. Application uses the ProfilesValue of the desired profile as
//             the setting for CAP_PROFILES. This means the ProfilesValue
//             will be one of the TWPO_* defines (e.g. TWPO_FILE01).
//
//		    Unless otherwise specified, start with a structure set to all zeros
//          (i.e. memset to 0), then fill in all required fields, and then fill
//          in any optional fields as desired    
//
//          To create a new profile (version 10.x+):
//          -----------------------------------------------------------------
//          - Application calls DG_CONTROL / DAT_PROFILES / MSG_PROFILECREATE
//          - Required:
//               szName       - Name of the new profile
//          - Optional:
//               szFilename   - Graphic filename associate with a new profile
//          - Errors:
//               If the new profile name is already existed then the driver 
//               driver will return TWCC_BADVALUE
//
//          To save the current driver settings (version 10.x+):
//          -----------------------------------------------------------------
//          - Application calls DG_CONTROL / DAT_PROFILES / MSG_PROFILESAVE
//          - Required: (none)
//          - Optional: (none)
//          - Errors: (none)
//
//          To delete the current profile (version 10.x+):
//          -----------------------------------------------------------------
//          - Application calls DG_CONTROL / DAT_PROFILES / MSG_PROFILEDELETE
//          - Required: (none)
//          - Optional:
//               szEnglish    - English name of profile that will be selected
//                              after deletion. If szEnglish is not specified,
//                              the driver will determine which profile to
//                              select. If you are deleting the last profile,
//                              then szEnglish should be blank
//          - Error:
//               If the profile is readonly or the profile state is 
//               TWPRF_PROFILESTATE_MODIFIED, the driver will return TWCC_DENIED 
//
//          To rename the current profile (version 10.x+):
//          -----------------------------------------------------------------
//          - Application calls DG_CONTROL / DAT_PROFILES / MSG_PROFILERENAME
//          - Required: At least specify one of the option below
//          - Optional: (can specify szCurrent or szFilename or both)
//               szName       - Name of the profile; if not supplied, the name
//                              will not be changed
//               szFilename   - Graphic filename associate with a new profile;
//                              if not supplied, the graphic will not be changed
//          - Error:
//               If the profile is readonly or the profile state is 
//               TWPRF_PROFILESTATE_MODIFIED, the driver will return TWCC_DENIED 
//
//          To restore all the supplied profiles to what was installed (v10.x+):
//          -----------------------------------------------------------------
//          - Application calls DG_CONTROL / DAT_PROFILES / MSG_PROFILERESTORE
//          - Required:
//               blSharedSettings    - Include settings shared across profiles
//                                     (e.g. Power Saver)
//          - Optional: (none)
//          - Error:
//               If profile state is TWPRF_PROFILESTATE_MODIFIED for one of the 
//               profiles, the driver will return TWCC_DENIED
//
//          To export profiles (version 10.x+):
//          -----------------------------------------------------------------
//          - Application calls DG_CONTROL / DAT_PROFILES / MSG_PROFILEEXPORT
//          - Required:
//               szFilename    - Name of the file to export
//          - Optional: (none)
//          - Error:
//               If profile state is TWPRF_PROFILESTATE_MODIFIED for one of the 
//               profiles, the driver will return TWCC_DENIED
//
//          To import profiles (version 10.x+):
//          -----------------------------------------------------------------
//          - Application calls DG_CONTROL / DAT_PROFILES / MSG_PROFILEIMPORT
//          This will remove all profiles that have been configured and
//          replace them with the imported profiles.
//          - Required:
//               szFilename    - Name of the file to import
//          - Optional: (none)
//          - Error:
//               If profile state is TWPRF_PROFILESTATE_MODIFIED for one of the 
//               profiles, the driver will return TWCC_DENIED
//
//          To set current profile (version 10.x+):
//          -----------------------------------------------------------------
//          - Application calls DG_CONTROL / DAT_PROFILES / MSG_PROFILESET
//          - Required:
//               szEnglish    - English name of the profile to be selected
//          - Optional: (none)
//          - Error: (none)
//
#define DAT_PROFILES                        0x8007
typedef struct
{
    TW_UINT16		ProfilesValue;			// CAP_PROFILES TWPO_* value
	TW_STR255		szFamily;				// Deprecated TWAIN 10.x+
    TW_STR255		szId;                   // unique profile ID
    TW_STR255		szEnglish;              // profile name in English
    TW_STR255		szCurrent;              // Deprecated TWAIN 10.x+
	TW_UINT32       Group;	                // group type (see TWPRF_GROUP_*)
    TW_BOOL			Readonly;               // cannot be modifed or deleted?
    TW_BOOL			Default;                // a default profile?
	TW_UINT32		DriverVersion;          // Deprecated TWAIN 10.x+
	TW_STR255		szMethod;               // Deprecated TWAIN 10.x+

	// the following are valid version 10.x+

	TW_STR255		szName;                 // profile name in the current language.
                                            // Uses the Ansi code page. The
                                            // language is based on CAP_LANGUAGE

	TW_STR255		szFilename;				// Full path and filename to a file	
                                            // The filename is either graphic filename
						                    // or profile filename based on the MSG
						                    // See the description above for more details

	TW_BOOL			blSharedSettings;		// Indicates whether the shared profile
											// settings are to be reset as well. 
	TW_UINT16		ProfileState;			// Profile state (see TWPRF_PROFILESTATE_*)

	TW_UINT8		Reserved[508];			// Room for more stuff, always set to 0.

} TW_PROFILES, *pTW_PROFILES;


// group types for TW_PROFILES
#define TWPRF_GROUP_ALL                     0


// The profile state. There can only be one profile in the
// profile list that is not normal.
#define TWPRF_PROFILESTATE_NORMAL           0   // Not selected or modified
#define TWPRF_PROFILESTATE_CURRENT          1   // Currently selected profile
#define TWPRF_PROFILESTATE_MODIFIED         2   // Currently selected and has been modified


//  Notes:  This operation is used by the application to
//		DG_CONTROL / DAT_QUERYSUPPORT/ MSG_GET  or MSG_GETCURRENT or
//		MSG_GETDEFAULT.  This allows the application to find out if the DAT
//		capability is supported or not.
#define DAT_QUERYSUPPORT                   0x800B

typedef struct {
    TW_UINT32	DG;
	TW_UINT16	DAT;
   	TW_UINT16	MSG[64];		//Return the array of supported MSG
}TW_QUERYSUPPORT, FAR *pTW_QUERYSUPPORT;


//
//  Gemini/Viper RAW Status (DG_CONTROL)...
//  Notes:  This command may be issued at any time. It reports
//          the raw data for the last error status reported by
//          the scanner.  These are device dependent values
//          which may be reported for servicing issues, but
//          should not be used to drive code...
//
#define DAT_STATUSRAW                       0x8001
typedef struct
{
    TW_INT32  LastToolkitStatus;
    TW_INT32  LastSenseData[3];
    TW_STR255 LastText;
    TW_INT32  CurrentState;
    TW_INT32  LastKDSStatus;
} TW_STATUSRAW, *pTW_STATUSRAW;


//
//  Window Management
//  Notes:  Use this structure to add and remove windows.
//			A window is an index to a set of cameras, such
//			as front_bitonal, front_color, etc...
//
#define DAT_WINDOW                          0x8008
typedef struct
{
    TW_UINT16    WindowCamera;              // CAP_WINDOWCAMERA value
    TW_UINT8     Reserved[1024];            // Room for more stuff
} TW_WINDOW, *pTW_WINDOW;


#define TWTY_FRAMEANGLE                     0x8001
typedef struct {
   TW_FIX32   Left;		// same as TW_FRAME.Left
   TW_FIX32   Top;		// same as TW_FRAME.Top
   TW_FIX32   Right;	// same as TW_FRAME.Right
   TW_FIX32   Bottom;	// same as TW_FRAME.Bottom
   TW_INT32   Angle;	// -3600000 to +3600000
} TW_FRAMEANGLE, FAR * pTW_FRAMEANGLE;



////////////////////////////////////////////////////////////////////////////////
//                      MSG Section
////////////////////////////////////////////////////////////////////////////////



//
//  All Disable/Enable UI...
//  Notes:  This message is added to DG_CONTROL / DAT_USERINTERFACE
//          to allow the application to temporarily disable access
//          to the Source's GUI.
//
#define MSG_DISABLEUI                       0x8003
#define MSG_ENABLEUI                        0x8004


//
//  All Enable Scanner...
//  Notes:  This message is added to DG_CONTROL / DAT_USERINTERFACE
//          to allow the user to enable the scanner.  The main use
//          for this command is to allow a filter writer to split
//          a MSG_ENABLEDS into a MSG_SETUPDS followed by a
//          MSG_ENABLESCANNER, allowing the filter to interrogate
//          the scanner about its current setting before moving paper.
//          This command is only permitted in State 4, and if ShowUI
//          is set to FALSE...
//
#define MSG_ENABLESCANNER                   0x8006


//
//  All Special Get...
//  Notes:  Used to get data out of sequence with the specification.
//          The only example at this time is use with DG_IMAGE / 
//          DAT_EXTIMAGEINFO...
//
#define MSG_GETSPECIAL                      0x8005


//
//  Profile messages...
//  Notes:  Use these with DG_CONTROL / DAT_PROFILES.
//
#define MSG_SAVEPROFILE                     0x8008 // Deprecated TWAIN 10.x+
#define MSG_DELETEPROFILE                   0x8009 // Deprecated TWAIN 10.x+
#define MSG_PROFILECREATE                   0x8015
#define MSG_PROFILEDELETE                   0x8016
#define MSG_PROFILESAVE                     0x8017
#define MSG_PROFILERENAME                   0x8018
#define MSG_PROFILERESTORE                  0x8019
#define MSG_PROFILEEXPORT                   0x801A
#define MSG_PROFILEIMPORT                   0x801B
#define MSG_PROFILESET                      0x801C


//
//  All Setup DS...
//  Notes:  This message is added to DG_CONTROL / DAT_USERINTERFACE
//          to allow the user download the current settings without
//          initiating a scanning session.  The state remains at 4,
//          no matter what the outcome of the command.
//          To select the current settings see CAP_PROFILES or CUSTOMDSDATA
//
#define MSG_SETUPDS                         0x8002


//
//  Window messages...
//  Notes:  Use these with DG_CONTROL / DAT_WINDOW.
//
#define MSG_ADDWINDOW                       0x8011
#define MSG_DELETEWINDOW                    0x8012


// OBS_JPEGQUALITY
// Notes:       Same as standard ICAP_JPEGQUALITY, do not use.
#define OBS_JPEGQUALITY                     0x8040


//
//  All Stop Feeder...
//  Notes:  This message is added to DG_CONTROL / DAT_PENDINGXFERS
//          to allow the user to turn off the transport.
//          Do not use this message, use MSG_STOPFEEDER...
//
#define OBS_STOPFEEDER                      0x8001



////////////////////////////////////////////////////////////////////////////////
//                      TW Section
////////////////////////////////////////////////////////////////////////////////



// Filter...
#define TWRC_FILTER_CONTINUE                0x8010


//
//  Prism/Wildfire Patchcode extensions...
//  Notes:  This section extends the patch codes to match those
//          supported by the Prism/Wildfire scanner family.  Note that
//          a transfer patch is interpreted to be a level 2 or
//          a level three, so even though it can be set, a
//          transfer patch is never received as part of the
//          extended image info data...
//
#define TWPCH_PATCHT2                       0x8001
#define TWPCH_PATCHT3                       0x8002
#define TWEJ_PATCHT2                        0x8001
#define TWEJ_PATCHT3                        0x8002


//
//  QuerySupport extensions...
//
#define TWQC_MACHINE                        0x1000
#define TWQC_BITONAL                        0x2000
#define TWQC_COLOR                          0x4000
#define TWQC_WINDOW                         0x8000


// Device Events
//
// The following are the device event supported by all drivers
// prior to version 9.3: TWDE_PAPERDOUBLEFEED and TWDE_PAPERJAM
//
// For version 9.3 drivers and up, the following is a custom
// CAP_DEVICEEVENT the application can ask the driver to send
// it when the lamps need to warmup prior to scanning. This
// event will be issued after a MSG_ENABLEDS if the lamps
// are not warmed up. The number of seconds before the lamps
// are ready will be in the TimeBeforeFirstCapture field of
// the TW_DEVICEVENT structure.
//
// NOTE: The number of seconds may be longer than what it
//       actually take. This could happen because an error
//       occurred (e.g. opening cover, cancelling) or for
//       models that do not have an accurate warmup value.
//       Because of this, if your application is displaying
//       a “please wait” message for the user, the message
//       needs to be closed when MSG_XFERREADY is received.
#define TWDE_LAMPWARMUP        	0x8002


// TWEI_HDR_BINARIZATIONQUALITY
//
// This conveys the quality level of the binarized image
// Some binarization methods can detect if the  binarization
// resulted in a noisy (or otherwise suspect) output.
// For those binarization method that don't detect this,
// 'normal' is returned.
#define TWBQ_NORMAL             0
#define TWBQ_NOISY              1


////////////////////////////////////////////////////////////////////////////////
//                      EXTIMAGEINFO Section
////////////////////////////////////////////////////////////////////////////////



//
//  Gemini/Inferno/Phoenix/Prism/Viper/Wildfire Custom TWEI_ fields for DAT_EXTIMAGEINFO...
//  Notes:  These fields return custom image information
//          taken from the image header (or footer)...
//
#define TWEI_HDR_PAGESIDE                   0x8001  // 0-front 1-rear
#define TWEI_HDR_DOCUMENTCOUNT              0x8002  // Count of pages (seeded by user)
#define TWEI_HDR_LENGTH                     0x8003  // Number of bytes of image data
#define TWEI_HDR_LEVEL                      0x8004  // Image Address Level
#define TWEI_HDR_MODE                       0x8005  // Gemini Mode (1-18)
#define TWEI_HDR_LINELENGTH                 0x8006  // Width
#define TWEI_HDR_PAGELENGTH                 0x8007  // Height
#define TWEI_HDR_COMPRESSION                0x8008  // ICAP_COMPRESSION TWCP_* value
#define TWEI_HDR_DATE                       0x8009  // YY/MM/DD
#define TWEI_HDR_TIME                       0x800A  // HH/MM/00
#define TWEI_HDR_ROLL                       0x800B  // Imagelink 990 Film Roll Number
#define TWEI_HDR_RESOLUTION                 0x800C  // Dots Per Inch (DPI)
#define TWEI_HDR_BITORDER                   0x800D  // Bit order in a byte
#define TWEI_HDR_SKEW                       0x800E  // (obsolete)
#define TWEI_HDR_MOMENTARYFLAG              0x800F  // Gemini Momentary
#define TWEI_HDR_LATCHEDFLAG                0x8010  // Gemini Latched
#define TWEI_HDR_BARCODE                    0x8011  // Gemini Barcode(s)
#define TWEI_HDR_DESKEW                     0x8012  // Deskew status
#define TWEI_HDR_DESKEWANGLE                0x8013  // Deskew angle
#define TWEI_HDR_POLARITY                   0x8014  // ICAP_PIXELFLAVOR TWPF_* value
#define TWEI_HDR_PRINTERSTRING              0x8015  // Viper/Prism/Wildfire printed string
#define TWEI_HDR_PRINTERINDEX               0x8016  // Kinda like the document count
#define TWEI_HDR_IMAGENUMBER                0x8017  // Count images this session
#define TWEI_HDR_PAGENUMBER                 0x8018  // Count sheets of paper this session
#define TWEI_HDR_PAGEIMAGENUMBER            0x8019  // Image count on a page (1 - 4)
#define TWEI_HDR_BOOKNAME_A                 0x801A  // Fixed field Gemini/Prism/Wildfire A
#define TWEI_HDR_BOOKNAME_B                 0x801B  // Fixed field Prism/Wildfire B
#define TWEI_HDR_BOOKNAME_C                 0x801C  // Fixed field Prism/Wildfire C
#define TWEI_HDR_BOOKNAME_D                 0x801D  // Fixed field Prism/Wildfire D
#define TWEI_HDR_IMAGEADDRESSSTRING         0x801E  // Prism/Wildfire image address string
#define TWEI_HDR_XOFFSET                    0x801F  // Left cropping offset
#define TWEI_HDR_YOFFSET                    0x8020  // Right cropping offset
#define TWEI_HDR_FEATUREPATCH               0x8021  // Feature patch (only image with patch: i200/i600)
#define TWEI_HDR_IMAGEADDRESSDEFS           0x8022  // Image Address field definitions
#define TWEI_HDR_PCARD_HEADER               0x8023  // Personality-Card Header
#define TWEI_HDR_PCARD_FOOTER               0x8024  // Personality-Card Footer
#define TWEI_HDR_TOKEN_COUNT                0x8025  // Alien token flag
#define TWEI_HDR_REGENERATION               0x8026  // Alien retry count
#define TWEI_HDR_IMAGESTATUS                0x8027  // Alien image status
#define TWEI_HDR_DITHER                     0x8028  // Bitonal dithering algorithm used
#define TWEI_HDR_PATCHDETECTED              0x8029  // Patch was found on the document
#define TWEI_HDR_FOLDEDCORNERPERCENTAGE     0x802A  // Phoenix folded corner percentage
#define TWEI_HDR_DESKEWCONFIDENCEFACTOR     0x802B  // Phoenix deskew confidence factor
#define TWEI_HDR_BITONALCONTRASTPERCENTAGE  0x802C  // Phoenix bitonal contrast percentage
#define TWEI_HDR_BITONALCONTRAST            0x802D  // Phoenix bitonal contrast
#define TWEI_HDR_BITONALTHRESHOLD           0x802E  // Phoenix bitonal threshold
#define TWEI_HDR_SUMHISTOGRAM               0x802F  // Phoenix sum historgram
#define TWEI_HDR_DIFFERENCEHISTOGRAM        0x8030  // Phoenix difference histogram
#define TWEI_HDR_GAMMATABLE                 0x8031  // Phoenix gamma table
#define TWEI_HDR_MULTIFEED                  0x8032  // Multifeed detected
#define TWEI_HDR_DESKEWANGLEACTUAL          0x8033  // Signed deskew angle to scanner precision
#define TWEI_HDR_RAWIMAGEHEADER             0x8034  // Raw image header from scanner
#define TWEI_HDR_LONGPAPERSEGMENT           0x8035  // Long paper segment number
#define TWEI_HDR_LONGPAPERLASTSEGMENT       0x8036  // Long pager last segment
#define TWEI_HDR_AUTOCOLORDETECTED          0x8037  // Auto color detected
#define TWEI_HDR_AUTOCOLORAMOUNT            0x8038  // Auto color amount
#define TWEI_HDR_AUTOCOLORTHRESHOLD         0x8039  // Auto color threshold
#define TWEI_HDR_XML                        0x803A  // <reportimage> data in XML format (see sample at end of file)
#define TWEI_HDR_DROPOUTSTATUS				0x803B	// ECDO Algorithm Status
#define TWEI_HDR_PROCESSINGSTATUS           0x803C  // Processing Status
#define TWEI_HDR_BINARIZATIONQUALITY        0x803D  // Conveys the quality level of the binarized image
#define TWEI_HDR_DUALSTACKINGSTACK          0x803E  // Which output tray a document was dropped into (i5000 only)
													// Only valid if dual stacking is enabled; legal values are 1 and 2



////////////////////////////////////////////////////////////////////////////////
//                      List of custom capabilities in numeric order
////////////////////////////////////////////////////////////////////////////////


/*
CAP_FEEDERKEEPALIVE                     0x8001
CAP_PAGESIZELIMIT                       0x8002
CAP_TRANSPORTTIMEOUT                    0x8003
CAP_WINDOWPOSITION                      0x8004
ICAP_LAMPSAVER                          0x8005
ICAP_OVERSCANX                          0x8006
ICAP_OVERSCANY                          0x8007
ICAP_FORCECOMPRESSION                   0x8008
CAP_PRINTERWRITESEQUENCESTRING          0x8009
CAP_PRINTERWRITESEQUENCESPACE           0x800A
CAP_PRINTERWRITESEQUENCESPACESTRING     0x800B
CAP_PRINTERWRITESEQUENCEMESSAGE1        0x800C
CAP_PRINTERWRITESEQUENCEMESSAGE2        0x800D
CAP_PRINTERWRITESEQUENCEMESSAGE3        0x800E
CAP_PRINTERWRITESEQUENCEMESSAGE4        0x800F
CAP_PRINTERWRITESEQUENCEMESSAGE5        0x8010
CAP_PRINTERWRITESEQUENCEMESSAGE6        0x8011
CAP_PRINTERWRITESEQUENCEMESSAGE7        0x8012
CAP_PRINTERWRITESEQUENCEMESSAGE8        0x8013
CAP_PRINTERWRITESEQUENCEMESSAGE9        0x8014
CAP_IMAGEADDRESS                        0x8015
CAP_IMAGEADDRESSENABLED                 0x8016
CAP_DOCUMENTCOUNT                       0x8017
CAP_DOCUMENTCOUNTENABLED                0x8018
CAP_MODE                                0x8019
CAP_PRINTERINDEXFORMAT                  0x801A
CAP_PRINTERINDEXDIGITS                  0x801B
CAP_PRINTERDATEDELIMITER                0x801C
CAP_CAMERAENABLE                        0x801D
CAP_CAMERAORDER                         0x801E
ICAP_HALFTONESQUALITY                   0x801F
ICAP_COLORTABLE                         0x8020
ICAP_VERTICALBLACKLINEREMOVAL           0x8021
ICAP_CROPPINGMODE                       0x8022
ICAP_ADDBORDER                          0x8023
ICAP_FILTERENUM                         0x8024
ICAP_FILTERTHRESHOLD                    0x8025
ICAP_FILTERBACKGROUND                   0x8026
CAP_ULTRASONICSENSITIVITY               0x8027
CAP_TRANSPORTTIMEOUTRESPONSE            0x8028
CAP_TRANSPORTAUTOSTART                  0x8029
ICAP_LAMPTIMEOUT                        0x802A
CAP_BATCHCOUNT                          0x802B
CAP_PAPERSOURCE                         0x802C
CAP_MULTIFEEDSOUND                      0x802D
ICAP_GRAYSCALE                          0x802E
CAP_ENERGYSTAR                          0x802F
CAP_BINARIZATION                        0x8030
CAP_PAGECOUNT                           0x8031
CAP_NOWAIT                              0x8032
CAP_PRINTERDATEFORMAT                   0x8033
CAP_PRINTERWRITESEQUENCEMESSAGE10       0x8034
CAP_PRINTERWRITESEQUENCEMESSAGE11       0x8035
CAP_PRINTERWRITESEQUENCEMESSAGE12       0x8036
CAP_FUNCTIONKEY1                        0x8037
CAP_FUNCTIONKEY2                        0x8038
CAP_FUNCTIONKEY3                        0x8039
CAP_FUNCTIONKEY4                        0x803A
CAP_LEVELTOFOLLOW1                      0x803B
CAP_LEVELTOFOLLOW2                      0x803C
CAP_LEVELTOFOLLOW3                      0x803D
CAP_IMAGEADDRESSTEMPLATES               0x803E
CAP_BATCHSTARTFUNCTION                  0x803F
OBS_JPEGQUALITY                         0x8040
ICAP_COLORSHARPENING                    0x8041
ICAP_FRAMELENGTHCONTROL                 0x8042
ICAP_FLIPBACKGROUNDCOLOR                0x8043
CAP_PRINTERFONT                         0x8044
CAP_PRINTERIMAGEADDRESSFORMAT           0x8045
CAP_PRINTERPOSITION                     0x8046
CAP_PRINTERIMAGEADDRESSLEVEL            0x8047
CAP_PRINTERWRITESEQUENCEINDEX           0x8048
CAP_PRINTERWRITESEQUENCE                0x8049
CAP_IMAGEADDRESS_A                      0x804A
CAP_IMAGEADDRESS_B                      0x804B
CAP_IMAGEADDRESS_C                      0x804C
CAP_IMAGEADDRESS_D                      0x804D
CAP_BATCHLEVEL                          0x804E
CAP_BATCHENDFUNCTION                    0x804F
// Reserved 0x8050 - 0x8053
CAP_ENABLECOLORPATCHCODE                0x8054
CAP_FIXEDDOCUMENTSIZE                   0x8055
CAP_DOUBLEFEEDSTOP                      0x8056
CAP_MULTIFEEDTHICKNESSDETECTION         0x8057
// Reserved 0x8058 - 0x8069
CAP_PCARDENABLED                        0x806A
CAP_IMAGEMAGNIFICATIONFACTOR            0x806B
CAP_INDICATORSWARMUP                    0x806C
CAP_TOGGLEPATCH                         0x806D
CAP_DOUBLEFEEDENDJOB                    0x806E
CAP_FEEDERMODE                          0x806F
CAP_FOLDEDCORNER                        0x8070
CAP_FOLDEDCORNERSENSITIVITY             0x8071
ICAP_GAMMAENABLED                       0x8074
CAP_EASYSTACKING                        0x8075
// Reserved 0x8076 - 0x8085
CAP_MULTIFEEDCOUNT                      0x8086
CAP_PATCHCOUNT                          0x8087
ICAP_FILTERPROCESSING                   0x8088
CAP_BACKGROUND                          0x8089
CAP_PRINTERWRITESEQUENCEMESSAGEINDEX    0x808A
CAP_CHECKDIGIT                          0x808B
CAP_BACKGROUNDFRONT                     0x808C
CAP_BACKGROUNDREAR                      0x808D
CAP_BACKGROUNDPLATEN                    0x808E
CAP_ULTRASONICSENSORCENTER              0x808F
CAP_ULTRASONICSENSORLEFT                0x8090
CAP_ULTRASONICSENSORRIGHT               0x8091
ICAP_AUTOCOLORAMOUNT                    0x8092
ICAP_AUTOCOLORCONTENT                   0x8093
ICAP_AUTOCOLORTHRESHOLD                 0x8094
ICAP_IMAGEEDGEFILL                      0x8095
ICAP_IMAGEEDGELEFT                      0x8096
ICAP_IMAGEEDGERIGHT                     0x8097
ICAP_IMAGEEDGETOP                       0x8098
ICAP_IMAGEEDGEBOTTOM                    0x8099
CAP_BLANKPAGE                           0x809A
CAP_BLANKPAGEMODE                       0x809B
CAP_BLANKPAGECOMPSIZEBW                 0x809C
CAP_BLANKPAGECOMPSIZEGRAY               0x809D
CAP_BLANKPAGECOMPSIZERGB                0x809E
CAP_PROFILES                            0x809F
ICAP_ANSELBRIGHTNESS                    0x80A0
ICAP_ANSELCONTRAST                      0x80A1
ICAP_ANSELHIGHLIGHT                     0x80A2
ICAP_ANSELMIDTONE                       0x80A3
ICAP_ANSELREMOVEREDEYE                  0x80A4
ICAP_ANSELRESTORECOLOR                  0x80A5
ICAP_ANSELSATURATECOLORS                0x80A6
ICAP_ANSELSHADOW                        0x80A7
ICAP_ANSELSHARPENIMAGE                  0x80A8
ICAP_ANSELSHASTA                        0x80A9
ICAP_PADDING                            0x80AA
ICAP_NEWWINDOWSIZE                      0x80AB
ICAP_DOCUMENTTYPE                       0x80AC
CAP_WINDOW                              0x80AD
ICAP_BACKGROUNDADJUSTMODE               0x80AE
ICAP_BACKGROUNDADJUSTAPPLYTO            0x80AF
ICAP_BACKGROUNDADJUSTAGGRESSIVENESS     0x80B0
ICAP_COLORBALANCEBLUE                   0x80B1
ICAP_COLORBALANCEGREEN                  0x80B2
ICAP_COLORBALANCERED                    0x80B3
CAP_WINDOWCAMERA                        0x80B4
ICAP_SKEWANGLE                          0x80B5
ICAP_MEDIATYPE                          0x80B6
CAP_SIDESDIFFERENT                      0x80B7
ICAP_ECDO                               0x80B8
ICAP_IMAGEEDGEFILLALLSIDES              0x80B9
CAP_MULTIFEEDRESPONSE                   0x80BA
ICAP_FRAMESANGLE                        0x80BB
CAP_PRINTERDATE                         0x80BC
CAP_PRINTERTIME                         0x80BD
CAP_PRINTERFONTFORMAT                   0x80BE
CAP_PATCHHEAD1                          0x80BF
CAP_PATCHHEAD2                          0x80C0
CAP_PATCHHEAD3                          0x80C1
CAP_PATCHHEAD4                          0x80C2
ICAP_ECDOTREATASCOLOR                   0x80C3
CAP_BLANKPAGECONTENT                    0x80C4
CAP_IMAGEMERGE                          0x80C5
ICAP_STREAKREMOVALENABLED               0x80C6
ICAP_STREAKREMOVALAGGRESSIVENESS        0x80C7
// Reserved 0x80C8 - 0x80FF
CAP_IMAGESDIFFERENT						0x8100
ICAP_PHYSICALHEIGHTADJUST               0x8101
ICAP_COLORSHARPEN						0x8102
ICAP_ECDOAGGRESSIVENESS					0x8103
ICAP_HOLEFILLENABLED                    0x8104
CAP_ULTRASONICSENSORLEFTCENTER          0x8105
CAP_ULTRASONICSENSORRIGHTCENTER         0x8106
CAP_ULTRASONICSENSORZONEHEIGHT          0x8107
ICAP_COLORBRIGHTNESSMODE                0x8108
ICAP_COLORBALANCEMODE                   0x8109
ICAP_COLORBALANCEAUTOMATICAGGRESSIVENESS 0x810A
CAP_SIMULATING                          0x810B
CAP_POWEROFFTIMEOUT                     0x810C
CAP_INTELLIGENTDOCUMENTPROTECTION		0x810D
ICAP_FOREGROUNDBOLDNESSAGGRESSIVENESS   0x810E
ICAP_FOREGROUNDBOLDNESSMODE             0x810F
CAP_DUALSTACKINGENABLED					0x8110
CAP_DUALSTACKINGLENGTHMODE				0x8111
CAP_DUALSTACKINGLENGTH1					0x8112
CAP_DUALSTACKINGLENGTH2					0x8113
CAP_DUALSTACKINGMULTIFEED				0x8114
CAP_DUALSTACKINGPATCHTRANSFER			0x8115
CAP_DUALSTACKINGPATCHTYPE1				0x8116
CAP_DUALSTACKINGPATCHTYPE2				0x8117
CAP_DUALSTACKINGPATCHTYPE3				0x8118
CAP_DUALSTACKINGPATCHTYPE4				0x8119
CAP_DUALSTACKINGPATCHTYPE6				0x811A
CAP_DUALSTACKINGSTACK					0x811B
*/

/*
DAT_STATUSRAW                           0x8001
DAT_PCARD                               0x8002
DAT_COLORTABLEMAP                       0x8003
DAT_DEVICEINFO                          0x8005
DAT_AUTOCOLORLEARN                      0x8006
DAT_PROFILES                            0x8007
DAT_WINDOW                              0x8008
DAT_CUSTOMDSDATAGROUP		            0x8009
DAT_ECDO				                0x800A
DAT_QUERYSUPPORT			            0x800B
DAT_LOG                                 0x800C
DAT_UTC                                 0x800D
DAT_OCPBUTTONS                          0x800E
*/

/*
OBS_STOPFEEDER                          0x8001
MSG_SETUPDS                             0x8002
MSG_DISABLEUI                           0x8003
MSG_ENABLEUI                            0x8004
MSG_GETSPECIAL                          0x8005
MSG_ENABLESCANNER                       0x8006
MSG_SAVEPROFILE                         0x8008 // Deprecated TWAIN 10.x+
MSG_DELETEPROFILE                       0x8009 // Deprecated TWAIN 10.x+
// Reserved 0x8007, 0x800A - 0x8010, 0x8013 - 0x8014
MSG_ADDWINDOW                           0x8011
MSG_DELETEWINDOW                        0x8012
MSG_PROFILECREATE                       0x8015
MSG_PROFILEDELETE                       0x8016
MSG_PROFILESAVE                         0x8017
MSG_PROFILERENAME                       0x8018
MSG_PROFILERESTORE                      0x8019
MSG_PROFILEEXPORT                       0x801A
MSG_PROFILEIMPORT                       0x801B
MSG_PROFILESET                          0x801C


*/

/*
TWRC_BUSY                               0x8001
TWRC_PCARD_CHECKSTATUS                  0x8002
TWRC_PCARD_FATAL                        0x8003
TWRC_PCARD_RETRYIMAGE                   0x8004
TWRC_PCARD_NOCARD                       0x8005
TWRC_FILTER_CONTINUE                    0x8010
*/

/*
This is a sample of the kind of data that comes back from
    DG_IMAGE / DAT_EXTIMAGEINFO / MSG_GET
when asking for TWEI_HDR_XML...

Results will vary depending on the kind of scanning session,
this example is from MSG_ENABLEDS...

<task id='00000000000000000009' reply='00000000000000000013' bytes='00000000000000001696'>
        <reportstatus>
                <status>success</status>
        </reportstatus>
        <reportimage>
                <cameraid>front</cameraid>
                <cameratype>front</cameratype>
                <windowid>window_002</windowid>
                <imageid>000001A.1</imageid>
                <autocolordetected>false</autocolordetected>
                <autocoloramount>1</autocoloramount>
                <autocolorthreshold>20</autocolorthreshold>
                <compressionmode>none</compressionmode>
                <deskewstatus>success</deskewstatus>
                <imagefilename>C:\ProgramData\kds_kodak\kds_i2900\twain\log.20120622172913994\hippo\tmp\hip631C.tmp</imagefilename>
                <imageformat>bw</imageformat>
                <imageoffsetx>324</imageoffsetx>
                <imageoffsety>1404</imageoffsety>
                <imagewidth>10296</imagewidth>
                <imageheight>13170</imageheight>
                <imagesize>471925</imagesize>
                <imageresolutionx>200_dpi</imageresolutionx>
                <imageskewangle>2197</imageskewangle>
                <invertcolor>false</invertcolor>
                <binarizationquality>normal</binarizationquality>
                <blankimagecontent>-1</blankimagecontent>
                <region>
                        <imageoffsetx>324</imageoffsetx>
                        <imageoffsety>1404</imageoffsety>
                        <imagewidth>10296</imagewidth>
                        <imageheight>13170</imageheight>
                        <imageskewangle>2197</imageskewangle>
                        <imageorthogonalangle>0_degrees</imageorthogonalangle>
                </region>
                <imagetype><![CDATA[normal]]></imagetype>
                <multifeed><![CDATA[false]]></multifeed>
                <patchdetected><![CDATA[false]]></patchdetected>
                <printedstring><![CDATA[]]></printedstring>
                <printerindex><![CDATA[0]]></printerindex>
                <outputimagecount>1</outputimagecount>
                <outputsheetcount>1</outputsheetcount>
                <outputsheetimagecount>1</outputsheetimagecount>
        </reportimage>
</task>

*/

#endif // KDSCUST_H
