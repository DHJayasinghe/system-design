﻿namespace BnA.IAM.Presentation.API.Common;

public enum SignInResponse
{
    InvalidCredential = 0,
    Success = 1,
    AccountLockOut = 2,
}