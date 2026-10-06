using LmCorbieUI;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace AddinArtama {
  public class FormatoPadrao {
    // exibir/ocultar
    static bool swDisplayPlanes;
    static bool swDisplayAxes;
    static bool swDisplayTemporaryAxes;
    static bool swDisplayCoordSystems;
    static bool swDisplayReferencePoints2;
    static bool swDisplayOrigins;
    static bool swDisplayDatumCoordSystems;
    static bool swDisplayCurves;
    static bool swDisplayPartingLines;
    static bool swDisplayAllAnnotations;
    static bool swDisplayCompAnnotations;
    static bool swHideShowSketchDimensions;
    static bool swDisplaySketches;
    static bool swViewSketchRelations;
    static bool swDisplaySketchPlanes;
    static bool swGridDisplay;
    static bool swDisplayLights;
    static bool swDisplayCameras;
    static bool swDisplayDecals;
    static bool swDisplayReferencePoints;
    static bool swDisplayLiveSections;
    static bool swShowDimensionNames;
    static bool swDisplayWeldBead;
    static bool swDisplayCenterOfMassSymbol;
    static bool swViewDispGlobalBBox;

    // text preferences
    // - Anotações
    static TextFormat swDetailingAnnotationTextFormat;
    static TextFormat swDetailingNoteTextFormat;
    static TextFormat swDetailingBalloonTextFormat;
    static TextFormat swDetailingDatumTextFormat;
    static TextFormat swDetailingSurfaceFinishTextFormat;
    static TextFormat swDetailingGeometricToleranceTextFormat;
    static TextFormat swDetailingWeldSymbolTextFormat;
    static TextFormat swDetailingLocationLabelTextFormat;
    // - Dimensões
    static TextFormat swDetailingDimensionTextFormat;
    static TextFormat swDetailingAngularRunningDimension;
    static TextFormat swDetailingLinearDimension;
    static TextFormat swDetailingDiameterDimension;
    static TextFormat swDetailingRadiusDimension;
    static TextFormat swDetailingHoleDimension;
    static TextFormat swDetailingAngleDimension;
    static TextFormat swDetailingChamferDimension;
    static TextFormat swDetailingOrdinateDimension;
    static TextFormat swDetailingArcLengthDimension;


    static TextFormat swSheetMetalBendNotesTextFormat;
    static TextFormat swDetailingTableTextFormat;
    static TextFormat swDetailingViewTextFormat;
    static TextFormat swDetailingSectionTextFormat;
    static TextFormat swDetailingSectionLabelNameTextFormat;
    static TextFormat swDetailingSectionLabelLabelTextFormat;
    static TextFormat swDetailingSectionLabelScaleTextFormat;
    static TextFormat swDetailingSectionLabelDelimiterTextFormat;
    static TextFormat swDetailingSectionView_RotationTextFormat;

    // toogle preferences
    static bool swDimensionsExtensionLineStyleSameAsLeaderRadius;
    static bool swDimensionsExtensionLineStyleSameAsLeaderHole;
    static bool swDetailingDimsCenterText;
    static bool swDetailingScaleWithDimHeight;
    static bool swSheetMetalBendNotesLeaderJustificationSnapping;
    static bool swDetailingOrthoView_AddViewLabelOnViewCreation;
    static bool swWeldmentEnableAutomaticCutList;
    static bool swWeldmentEnableAutomaticUpdate;
    static bool swDetailingMiscView_AddViewLabelOnViewCreation;
    static bool swDisableDerivedConfigurations;
    static bool swWeldmentRenameCutlistDescriptionPropertyValue;
    static bool swDisableWeldmentConfigStrings;
    static bool swDetailingScaleWithSectionTextHeight;
    static bool swDetailingSectionViewLabels_PerStandard;
    static bool swDetailingDetailViewLabels_PerStandard;
    static bool swDetailingAuxViewLabels_PerStandard;
    static bool swDetailingOrthoViewLabels_PerStandard;
    static bool swDetailingMiscView_PerStandard;

    // string preferences
    static string swSheetMetalBendNotesLayer;
    static string swCenterLineLayer;
    static string swCenterMarkLayer;
    static string swDetailingLayer;
    static string swDetailingLayerNote;
    static string swDetailingLayerBalloon;
    static string swDetailingLayerAngularRunningDimension;
    static string swDetailingLayerBendTable;
    static string swDetailingLayerLinearDimension;
    static string swDetailingLayerDiameterDimension;
    static string swDetailingLayerHoleDimension;
    static string swDetailingLayerChamferDimension;
    static string swDetailingLayerOrdinateDimension;
    static string swDetailingLayerAngleDimension;
    static string swDetailingLayerDatum;
    static string swDetailingLayerSurfaceFinishSymbol;
    static string swDetailingLayerGeometricTolerance;
    static string swDetailingLayerWeldSymbol;
    static string swDetailingLayerBillOfMaterial;
    static string swDetailingLayerRevisionTable;
    static string swDetailingLayerGeneralTable;
    static string swDetailingLayerSectionView;
    static string swDetailingLayerDetailView;
    static string swDetailingLayerAuxiliaryView;
    static string swDetailingLayerHoleTable;
    static string swDetailingLayerArcLengthDimension;
    static string swDetailingLayerOrthoView;
    static string swDetailingLayerPunchTable;
    static string swDetailingLayerLocationLabel;
    static string swDetailingLayerRevisionCloud;
    static string swDetailingLayerMiscView;
    static string swDetailingLayerWeldTable;
    static string swDetailingLayerRadiusDimension;
    static string swDetailingSectionViewLabels;

    // double preferences
    static double swDetailingBreakLineGap;
    static double swDetailingCenterOfMassSize;
    static double swDetailingArrowWidth;
    static double swDetailingArrowHeight;
    static double swDetailingArrowLength;
    static double swDetailingObjectToDimOffset;
    static double swDetailingDimToDimOffset;
    static double swDetailingWitnessLineGap;
    static double swDetailingWitnessLineExtension;
    static double swDetailingBOMBalloonCustomSize;
    static double swDetailingBOMStackedBalloonCustomSize;

    // integer preferences sem especificação
    static int swDetailingNotesLeaderStyle;
    static int swDetailingBalloonLeaderStyle;
    static int swDetailingBalloonAutoBalloons;
    static int swUnitsLinearDecimalPlaces;
    static int swUnitsDualLinearDecimalPlaces;
    static int swUnitsDualLinear;
    static int swUnitsAngular;
    static int swUnitsAngularDecimalPlaces;
    static int swUnitsMassPropLength;
    static int swUnitsMassPropDecimalPlaces;
    static int swUnitsMassPropMass;
    static int swUnitsMassPropVolume;
    static int swLineFontTangentEdgesStyle;
    static int swLineFontBendLineUpStyle;
    static int swDetailingAngularDimPrecision;
    static int swSheetMetalColorBendLinesUp;
    static int swSheetMetalColorBendLinesDown;
    static int swSheetMetalColorFlatPatternSketch;
    static int swRevisionTableSymbolShape;
    static int swRevisionTableTagStyle;
    static int swUnitSystem;
    static int swUnitsLinear;
    static int swUnitsTimeDecimalPlaces;
    static int swLineFontSectionLineStyle;
    static int swLineFontSectionLineThickness;
    static int swDetailingSectionViewLineStyleDisplay;
    static int swDetailingVirtualSharpStyle;
    static int swLineFontDetailCircleStyle;
    static int swDetailingBOMBalloonFit;
    static int swDetailingBOMStackedBalloonFit;

    // integer preferences com especificação
    static int swDetailingLinearDimPrecisionDimension;
    static int swDetailingLinearDimPrecisionDiameterDimension;
    static int swDetailingLinearDimPrecisionRadiusDimension;
    static int swDetailingLinearDimPrecisionHoleDimension;
    static int swDetailingLinearDimPrecisionChamferDimension;
    static int swDetailingLinearDimPrecisionOrdinateDimension;
    static int swDetailingLinearDimPrecisionArcLengthDimension;
    static int swDetailingLinearDimPrecisionLinearDimension;
    static int swDetailingAltLinearDimPrecisionDimension;
    static int swDetailingAltLinearDimPrecisionLinearDimension;
    static int swDetailingAltLinearDimPrecisionDiameterDimension;
    static int swDetailingAltLinearDimPrecisionRadiusDimension;
    static int swDetailingAltLinearDimPrecisionHoleDimension;
    static int swDetailingAltLinearDimPrecisionChamferDimension;
    static int swDetailingAltLinearDimPrecisionOrdinateDimension;
    static int swDetailingAltLinearDimPrecisionArcLengthDimension;
    static int swDetailingDimensionTextAndLeaderStyleDiameterDimension;
    static int swDetailingDimensionTextAndLeaderStyleRadiusDimension;
    static int swDetailingDimensionTextAndLeaderStyleHoleDimension;
    static int swDimensionsExtensionLineStyleRadiusDimension;
    static int swDimensionsExtensionLineStyleHoleDimension;
    static int swDimensionsExtensionLineStyleChanferDimension;
    static int swDimensionsExtensionLineStyleThicknessRadiusDimension;
    static int swDimensionsExtensionLineStyleThicknessHoleDimension;
    static int swDimensionsExtensionLineStyleThicknessChamferDimension;
    static int swDetailingAngularDimPrecisionAngleDimension;
    static int swDetailingAngularDimPrecisionChamferDimension;
    static int swDetailingDimTrailingZeroDimension;
    static int swDetailingAngleTrailingZero;
    static int swDetailingAngleTrailingZeroTolerance;

    public static void ChangeFileProps(ModelDoc2 swModel) {
      try {
        if (swDetailingDimensionTextFormat == null)
          return;

        // exibir/ocultar
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayPlanes, swDisplayPlanes);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAxes, swDisplayAxes);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayTemporaryAxes, swDisplayTemporaryAxes);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCoordSystems, swDisplayCoordSystems);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayReferencePoints2, swDisplayReferencePoints2);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayOrigins, swDisplayOrigins);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayDatumCoordSystems, swDisplayDatumCoordSystems);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCurves, swDisplayCurves);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayPartingLines, swDisplayPartingLines);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAllAnnotations, swDisplayAllAnnotations);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCompAnnotations, swDisplayCompAnnotations);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swHideShowSketchDimensions, swHideShowSketchDimensions);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplaySketches, swDisplaySketches);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swViewSketchRelations, swViewSketchRelations);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplaySketchPlanes, swDisplaySketchPlanes);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swGridDisplay, swGridDisplay);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayLights, swDisplayLights);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCameras, swDisplayCameras);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayDecals, swDisplayDecals);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayReferencePoints, swDisplayReferencePoints);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayLiveSections, swDisplayLiveSections);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swShowDimensionNames, swShowDimensionNames);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayWeldBead, swDisplayWeldBead);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCenterOfMassSymbol, swDisplayCenterOfMassSymbol);
        swModel.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swViewDispGlobalBBox, swViewDispGlobalBBox);

        // text preferences
        // - Anotações
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingAnnotationTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingAnnotationTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingNoteTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingNoteTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingBalloonTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBalloonTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDatumTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingDatumTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSurfaceFinishTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSurfaceFinishTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingGeometricToleranceTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingGeometricToleranceTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingWeldSymbolTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingWeldSymbolTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingLocationLabelTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingLocationLabelTextFormat);
        // - Dimensões
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingDimension, swDetailingDimensionTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingAngularRunningDimension, swDetailingAngularRunningDimension);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingLinearDimension, swDetailingLinearDimension);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingDiameterDimension);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingRadiusDimension);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingHoleDimension);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingAngleDimension);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingChamferDimension);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension, swDetailingOrdinateDimension);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension, swDetailingArcLengthDimension);

        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swSheetMetalBendNotesTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalBendNotesTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingTableTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingTableTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingViewTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingViewTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelNameTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionLabelNameTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelLabelTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionLabelLabelTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelScaleTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionLabelScaleTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelDelimiterTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionLabelDelimiterTextFormat);
        swModel.Extension.SetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionView_RotationTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionView_RotationTextFormat);

        // toogle preferences
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDimensionsExtensionLineStyleSameAsLeader, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDimensionsExtensionLineStyleSameAsLeaderRadius);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDimensionsExtensionLineStyleSameAsLeader, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDimensionsExtensionLineStyleSameAsLeaderHole);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingDimsCenterText, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingDimsCenterText);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingScaleWithDimHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingScaleWithDimHeight);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swSheetMetalBendNotesLeaderJustificationSnapping, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalBendNotesLeaderJustificationSnapping);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingOrthoView_AddViewLabelOnViewCreation, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingOrthoView_AddViewLabelOnViewCreation);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentEnableAutomaticCutList, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swWeldmentEnableAutomaticCutList);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentEnableAutomaticUpdate, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swWeldmentEnableAutomaticUpdate);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingMiscView_AddViewLabelOnViewCreation, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingMiscView_AddViewLabelOnViewCreation);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisableDerivedConfigurations, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDisableDerivedConfigurations);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentRenameCutlistDescriptionPropertyValue, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swWeldmentRenameCutlistDescriptionPropertyValue);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisableWeldmentConfigStrings, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDisableWeldmentConfigStrings);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingScaleWithSectionTextHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingScaleWithSectionTextHeight);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingSectionViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionViewLabels_PerStandard);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingDetailViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingDetailViewLabels_PerStandard);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingAuxViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingAuxViewLabels_PerStandard);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingOrthoViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingOrthoViewLabels_PerStandard);
        swModel.Extension.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingMiscView_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingMiscView_PerStandard);

        // string preferences
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swSheetMetalBendNotesLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalBendNotesLayer);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swCenterLineLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swCenterLineLayer);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swCenterMarkLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swCenterMarkLayer);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingLayer);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingNote, swDetailingLayerNote);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBalloon, swDetailingLayerBalloon);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAngularRunningDimension, swDetailingLayerAngularRunningDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBendTable, swDetailingLayerBendTable);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingLinearDimension, swDetailingLayerLinearDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingLayerDiameterDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingLayerHoleDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingLayerChamferDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension, swDetailingLayerOrdinateDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingLayerAngleDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDatum, swDetailingLayerDatum);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingSurfaceFinishSymbol, swDetailingLayerSurfaceFinishSymbol);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingGeometricTolerance, swDetailingLayerGeometricTolerance);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingWeldSymbol, swDetailingLayerWeldSymbol);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBillOfMaterial, swDetailingLayerBillOfMaterial);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRevisionTable, swDetailingLayerRevisionTable);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingGeneralTable, swDetailingLayerGeneralTable);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingSectionView, swDetailingLayerSectionView);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDetailView, swDetailingLayerDetailView);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAuxiliaryView, swDetailingLayerAuxiliaryView);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingHoleTable, swDetailingLayerHoleTable);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension, swDetailingLayerArcLengthDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingOrthoView, swDetailingLayerOrthoView);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingPunchTable, swDetailingLayerPunchTable);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingLocationLabel, swDetailingLayerLocationLabel);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRevisionCloud, swDetailingLayerRevisionCloud);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingMiscView, swDetailingLayerMiscView);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingWeldTable, swDetailingLayerWeldTable);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingLayerRadiusDimension);
        swModel.Extension.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingSectionViewLabels_CustomName, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionViewLabels);

        // double preferences
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBreakLineGap, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBreakLineGap);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingCenterOfMassSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingCenterOfMassSize);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowWidth, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingArrowWidth);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingArrowHeight);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowLength, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingArrowLength);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingObjectToDimOffset, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingObjectToDimOffset);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingDimToDimOffset, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingDimToDimOffset);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingWitnessLineGap, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingWitnessLineGap);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingWitnessLineExtension, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingWitnessLineExtension);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBOMBalloonCustomSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBOMBalloonCustomSize);
        swModel.Extension.SetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBOMStackedBalloonCustomSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBOMStackedBalloonCustomSize);

        // integer preferences sem especificação
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingNotesLeaderStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingNotesLeaderStyle);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBalloonLeaderStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBalloonLeaderStyle);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBalloonAutoBalloons, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBalloonAutoBalloons);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsLinearDecimalPlaces);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsDualLinearDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsDualLinearDecimalPlaces);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsDualLinear, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsDualLinear);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsAngular, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsAngular);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsAngularDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsAngularDecimalPlaces);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropLength, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsMassPropLength);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsMassPropDecimalPlaces);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropMass, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsMassPropMass);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropVolume, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsMassPropVolume);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontTangentEdgesStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontTangentEdgesStyle);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontBendLineUpStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontBendLineUpStyle);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingAngularDimPrecision);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorBendLinesUp, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalColorBendLinesUp);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorBendLinesDown, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalColorBendLinesDown);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorFlatPatternSketch, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swSheetMetalColorFlatPatternSketch);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swRevisionTableSymbolShape, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swRevisionTableSymbolShape);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swRevisionTableTagStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swRevisionTableTagStyle);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitSystem, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitSystem);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsLinear, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsLinear);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsTimeDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swUnitsTimeDecimalPlaces);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontSectionLineStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontSectionLineStyle);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontSectionLineThickness, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontSectionLineThickness);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingSectionViewLineStyleDisplay, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingSectionViewLineStyleDisplay);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingVirtualSharpStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingVirtualSharpStyle);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontDetailCircleStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swLineFontDetailCircleStyle);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBOMBalloonFit, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBOMBalloonFit);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBOMStackedBalloonFit, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, swDetailingBOMStackedBalloonFit);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingSectionViewLabels_Name, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, (int)swDetailingViewLabelsName_e.swDetailingViewLabelsName_custom);

        // integer preferences com especificação
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDimension, swDetailingLinearDimPrecisionDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingLinearDimPrecisionDiameterDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingLinearDimPrecisionRadiusDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingLinearDimPrecisionHoleDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingLinearDimPrecisionChamferDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension, swDetailingLinearDimPrecisionOrdinateDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension, swDetailingLinearDimPrecisionArcLengthDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingLinearDimension, swDetailingLinearDimPrecisionLinearDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDimension, swDetailingAltLinearDimPrecisionDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingLinearDimension, swDetailingAltLinearDimPrecisionLinearDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingAltLinearDimPrecisionDiameterDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingAltLinearDimPrecisionRadiusDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingAltLinearDimPrecisionHoleDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingAltLinearDimPrecisionChamferDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension, swDetailingAltLinearDimPrecisionOrdinateDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension, swDetailingAltLinearDimPrecisionArcLengthDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingDiameterDimension, swDetailingDimensionTextAndLeaderStyleDiameterDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDetailingDimensionTextAndLeaderStyleRadiusDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDetailingDimensionTextAndLeaderStyleHoleDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDimensionsExtensionLineStyleRadiusDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDimensionsExtensionLineStyleHoleDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDimensionsExtensionLineStyleChanferDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingRadiusDimension, swDimensionsExtensionLineStyleThicknessRadiusDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingHoleDimension, swDimensionsExtensionLineStyleThicknessHoleDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDimensionsExtensionLineStyleThicknessChamferDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingAngularDimPrecisionAngleDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension, swDetailingAngularDimPrecisionChamferDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimTrailingZero, (int)swUserPreferenceOption_e.swDetailingDimension, swDetailingDimTrailingZeroDimension);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngleTrailingZero, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingAngleTrailingZero);
        swModel.Extension.SetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngleTrailingZeroTolerance, (int)swUserPreferenceOption_e.swDetailingAngleDimension, swDetailingAngleTrailingZeroTolerance);

        swModel.ForceRebuild3(TopOnly: true);
      } catch (Exception ex) {
        Toast.Error($"Erro ao alterar propriedades do desenho\n\n{ex.Message}");
      }
    }

    public static void GetDefaultFileProps(ModelDoc2 swModelTemplate) {
      try {
        // exibir/ocultar
        swDisplayPlanes = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayPlanes);
        swDisplayAxes = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAxes);
        swDisplayTemporaryAxes = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayTemporaryAxes);
        swDisplayCoordSystems = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCoordSystems);
        swDisplayReferencePoints2 = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayReferencePoints2);
        swDisplayOrigins = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayOrigins);
        swDisplayDatumCoordSystems = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayDatumCoordSystems);
        swDisplayCurves = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCurves);
        swDisplayPartingLines = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayPartingLines);
        swDisplayAllAnnotations = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAllAnnotations);
        swDisplayCompAnnotations = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCompAnnotations);
        swHideShowSketchDimensions = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swHideShowSketchDimensions);
        swDisplaySketches = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplaySketches);
        swViewSketchRelations = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swViewSketchRelations);
        swDisplaySketchPlanes = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplaySketchPlanes);
        swGridDisplay = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swGridDisplay);
        swDisplayLights = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayLights);
        swDisplayCameras = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCameras);
        swDisplayDecals = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayDecals);
        swDisplayReferencePoints = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayReferencePoints);
        swDisplayLiveSections = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayLiveSections);
        swShowDimensionNames = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swShowDimensionNames);
        swDisplayWeldBead = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayWeldBead);
        swDisplayCenterOfMassSymbol = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayCenterOfMassSymbol);
        swViewDispGlobalBBox = swModelTemplate.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swViewDispGlobalBBox);

        // text preferences
        // - Anotações
        swDetailingAnnotationTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingAnnotationTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingNoteTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingNoteTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingBalloonTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingBalloonTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingDatumTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDatumTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSurfaceFinishTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSurfaceFinishTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingGeometricToleranceTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingGeometricToleranceTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingWeldSymbolTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingWeldSymbolTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingLocationLabelTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingLocationLabelTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        // - Dimensões
        swDetailingDimensionTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingDimension);
        swDetailingAngularRunningDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingAngularRunningDimension);
        swDetailingLinearDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingLinearDimension);
        swDetailingDiameterDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        swDetailingRadiusDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        swDetailingHoleDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        swDetailingAngleDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingAngleDimension);
        swDetailingChamferDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        swDetailingOrdinateDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension);
        swDetailingArcLengthDimension = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingDimensionTextFormat, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension);

        swSheetMetalBendNotesTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swSheetMetalBendNotesTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingTableTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingTableTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingViewTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingViewTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSectionTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSectionLabelNameTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelNameTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSectionLabelLabelTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelLabelTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSectionLabelScaleTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelScaleTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSectionLabelDelimiterTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionLabelDelimiterTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSectionView_RotationTextFormat = swModelTemplate.Extension.GetUserPreferenceTextFormat((int)swUserPreferenceTextFormat_e.swDetailingSectionView_RotationTextFormat, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);

        // toogle preferences
        swDimensionsExtensionLineStyleSameAsLeaderRadius = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDimensionsExtensionLineStyleSameAsLeader, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        swDimensionsExtensionLineStyleSameAsLeaderHole = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDimensionsExtensionLineStyleSameAsLeader, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        swDetailingDimsCenterText = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingDimsCenterText, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingScaleWithDimHeight = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingScaleWithDimHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swSheetMetalBendNotesLeaderJustificationSnapping = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swSheetMetalBendNotesLeaderJustificationSnapping, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingOrthoView_AddViewLabelOnViewCreation = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingOrthoView_AddViewLabelOnViewCreation, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swWeldmentEnableAutomaticCutList = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentEnableAutomaticCutList, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swWeldmentEnableAutomaticUpdate = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentEnableAutomaticUpdate, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingMiscView_AddViewLabelOnViewCreation = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingMiscView_AddViewLabelOnViewCreation, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDisableDerivedConfigurations = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisableDerivedConfigurations, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swWeldmentRenameCutlistDescriptionPropertyValue = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swWeldmentRenameCutlistDescriptionPropertyValue, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDisableWeldmentConfigStrings = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisableWeldmentConfigStrings, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingScaleWithSectionTextHeight = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingScaleWithSectionTextHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSectionViewLabels_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingSectionViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingDetailViewLabels_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingDetailViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingAuxViewLabels_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingAuxViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingOrthoViewLabels_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingOrthoViewLabels_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingMiscView_PerStandard = swModelTemplate.Extension.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDetailingMiscView_PerStandard, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);

        // string preferences
        swSheetMetalBendNotesLayer = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swSheetMetalBendNotesLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swCenterLineLayer = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swCenterLineLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swCenterMarkLayer = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swCenterMarkLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingLayer = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingLayerNote = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingNote);
        swDetailingLayerBalloon = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBalloon);
        swDetailingLayerAngularRunningDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAngularRunningDimension);
        swDetailingLayerBendTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBendTable);
        swDetailingLayerLinearDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingLinearDimension);
        swDetailingLayerDiameterDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        swDetailingLayerHoleDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        swDetailingLayerChamferDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        swDetailingLayerOrdinateDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension);
        swDetailingLayerAngleDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAngleDimension);
        swDetailingLayerDatum = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDatum);
        swDetailingLayerSurfaceFinishSymbol = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingSurfaceFinishSymbol);
        swDetailingLayerGeometricTolerance = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingGeometricTolerance);
        swDetailingLayerWeldSymbol = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingWeldSymbol);
        swDetailingLayerBillOfMaterial = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingBillOfMaterial);
        swDetailingLayerRevisionTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRevisionTable);
        swDetailingLayerGeneralTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingGeneralTable);
        swDetailingLayerSectionView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingSectionView);
        swDetailingLayerDetailView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingDetailView);
        swDetailingLayerAuxiliaryView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingAuxiliaryView);
        swDetailingLayerHoleTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingHoleTable);
        swDetailingLayerArcLengthDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension);
        swDetailingLayerOrthoView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingOrthoView);
        swDetailingLayerPunchTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingPunchTable);
        swDetailingLayerLocationLabel = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingLocationLabel);
        swDetailingLayerRevisionCloud = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRevisionCloud);
        swDetailingLayerMiscView = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingMiscView);
        swDetailingLayerWeldTable = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingWeldTable);
        swDetailingLayerRadiusDimension = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingLayer, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        swDetailingSectionViewLabels = swModelTemplate.Extension.GetUserPreferenceString((int)swUserPreferenceStringValue_e.swDetailingSectionViewLabels_CustomName, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);

        // double preferences
        swDetailingBreakLineGap = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBreakLineGap, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingCenterOfMassSize = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingCenterOfMassSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingArrowWidth = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowWidth, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingArrowHeight = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowHeight, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingArrowLength = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingArrowLength, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingObjectToDimOffset = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingObjectToDimOffset, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingDimToDimOffset = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingDimToDimOffset, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingWitnessLineGap = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingWitnessLineGap, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingWitnessLineExtension = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingWitnessLineExtension, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingBOMBalloonCustomSize = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBOMBalloonCustomSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingBOMStackedBalloonCustomSize = swModelTemplate.Extension.GetUserPreferenceDouble((int)swUserPreferenceDoubleValue_e.swDetailingBOMStackedBalloonCustomSize, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);

        // integer preferences sem especificação
        swDetailingNotesLeaderStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingNotesLeaderStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingBalloonLeaderStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBalloonLeaderStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingBalloonAutoBalloons = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBalloonAutoBalloons, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsLinearDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsDualLinearDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsDualLinearDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsDualLinear = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsDualLinear, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsAngular = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsAngular, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsAngularDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsAngularDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsMassPropLength = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropLength, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsMassPropDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsMassPropMass = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropMass, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsMassPropVolume = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsMassPropVolume, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swLineFontTangentEdgesStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontTangentEdgesStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swLineFontBendLineUpStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontBendLineUpStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingAngularDimPrecision = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swSheetMetalColorBendLinesUp = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorBendLinesUp, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swSheetMetalColorBendLinesDown = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorBendLinesDown, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swSheetMetalColorFlatPatternSketch = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swSheetMetalColorFlatPatternSketch, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swRevisionTableSymbolShape = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swRevisionTableSymbolShape, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swRevisionTableTagStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swRevisionTableTagStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitSystem = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitSystem, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsLinear = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsLinear, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swUnitsTimeDecimalPlaces = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swUnitsTimeDecimalPlaces, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swLineFontSectionLineStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontSectionLineStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swLineFontSectionLineThickness = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontSectionLineThickness, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingSectionViewLineStyleDisplay = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingSectionViewLineStyleDisplay, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingVirtualSharpStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingVirtualSharpStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swLineFontDetailCircleStyle = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swLineFontDetailCircleStyle, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingBOMBalloonFit = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBOMBalloonFit, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);
        swDetailingBOMStackedBalloonFit = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingBOMStackedBalloonFit, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified);

        // integer preferences com especificação
        swDetailingLinearDimPrecisionDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDimension);
        swDetailingLinearDimPrecisionDiameterDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        swDetailingLinearDimPrecisionRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        swDetailingLinearDimPrecisionHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        swDetailingLinearDimPrecisionChamferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        swDetailingLinearDimPrecisionOrdinateDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension);
        swDetailingLinearDimPrecisionArcLengthDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension);
        swDetailingLinearDimPrecisionLinearDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingLinearDimension);
        swDetailingAltLinearDimPrecisionDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDimension);
        swDetailingAltLinearDimPrecisionLinearDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingLinearDimension);
        swDetailingAltLinearDimPrecisionDiameterDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        swDetailingAltLinearDimPrecisionRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        swDetailingAltLinearDimPrecisionHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        swDetailingAltLinearDimPrecisionChamferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        swDetailingAltLinearDimPrecisionOrdinateDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingOrdinateDimension);
        swDetailingAltLinearDimPrecisionArcLengthDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAltLinearDimPrecision, (int)swUserPreferenceOption_e.swDetailingArcLengthDimension);
        swDetailingDimensionTextAndLeaderStyleDiameterDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingDiameterDimension);
        swDetailingDimensionTextAndLeaderStyleRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        swDetailingDimensionTextAndLeaderStyleHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimensionTextAndLeaderStyle, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        swDimensionsExtensionLineStyleRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        swDimensionsExtensionLineStyleHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        swDimensionsExtensionLineStyleChanferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyle, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        swDimensionsExtensionLineStyleThicknessRadiusDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingRadiusDimension);
        swDimensionsExtensionLineStyleThicknessHoleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingHoleDimension);
        swDimensionsExtensionLineStyleThicknessChamferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDimensionsExtensionLineStyleThickness, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        swDetailingAngularDimPrecisionAngleDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingAngleDimension);
        swDetailingAngularDimPrecisionChamferDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngularDimPrecision, (int)swUserPreferenceOption_e.swDetailingChamferDimension);
        swDetailingDimTrailingZeroDimension = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingDimTrailingZero, (int)swUserPreferenceOption_e.swDetailingDimension);
        swDetailingAngleTrailingZero = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngleTrailingZero, (int)swUserPreferenceOption_e.swDetailingAngleDimension);
        swDetailingAngleTrailingZeroTolerance = swModelTemplate.Extension.GetUserPreferenceInteger((int)swUserPreferenceIntegerValue_e.swDetailingAngleTrailingZeroTolerance, (int)swUserPreferenceOption_e.swDetailingAngleDimension);

      } catch (Exception ex) {
        Toast.Error($"Erro ao pegar propriedades do desenho\n\n{ex.Message}");
      }
    }

  }
}